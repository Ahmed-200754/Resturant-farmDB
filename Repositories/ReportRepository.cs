using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models.ViewModels;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _connectionString;

        public ReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<CropReport?> GetHighestOrderedCropAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT TOP 1 c.CropName, COUNT(od.OrderDetailID) AS TotalOrders 
                FROM OrderDetails od 
                INNER JOIN HarvestBatches hb ON od.BatchID = hb.BatchID 
                INNER JOIN Crops c ON hb.CropID = c.CropID 
                GROUP BY c.CropID, c.CropName 
                ORDER BY TotalOrders DESC", connection);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new CropReport
                {
                    CropName = reader.GetString(0),
                    TotalOrders = reader.GetInt32(1)
                };
            }
            return null;
        }

        public async Task<List<FarmRevenue>> GetTotalRevenuePerFarmAsync()
        {
            var revenues = new List<FarmRevenue>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT f.FarmID, f.FarmName, COALESCE(SUM(od.UnitPrice * od.Quantity), 0) AS TotalRevenue 
                FROM Farm f 
                LEFT JOIN HarvestBatches hb ON f.FarmID = hb.FarmID 
                LEFT JOIN OrderDetails od ON hb.BatchID = od.BatchID 
                GROUP BY f.FarmID, f.FarmName 
                ORDER BY TotalRevenue DESC", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                revenues.Add(new FarmRevenue
                {
                    FarmID = reader.GetInt32(0),
                    FarmName = reader.GetString(1),
                    TotalRevenue = reader.GetDouble(2)
                });
            }
            return revenues;
        }

        public async Task<List<FarmRevenue>> GetTopFarmsByRevenueAsync()
        {
            var revenues = new List<FarmRevenue>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT TOP 5 f.FarmID, f.FarmName, COALESCE(SUM(od.UnitPrice * od.Quantity), 0) AS TotalRevenue 
                FROM Farm f 
                LEFT JOIN HarvestBatches hb ON f.FarmID = hb.FarmID 
                LEFT JOIN OrderDetails od ON hb.BatchID = od.BatchID 
                GROUP BY f.FarmID, f.FarmName 
                ORDER BY TotalRevenue DESC", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                revenues.Add(new FarmRevenue
                {
                    FarmID = reader.GetInt32(0),
                    FarmName = reader.GetString(1),
                    TotalRevenue = reader.GetDouble(2)
                });
            }
            return revenues;
        }

        public async Task<List<Models.Farm>> GetInactiveFarmsAsync(int year, int month)
        {
            var farms = new List<Models.Farm>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT f.FarmID, f.FarmName 
                FROM Farm f 
                WHERE f.FarmID NOT IN (
                    SELECT DISTINCT hb.FarmID 
                    FROM HarvestBatches hb 
                    WHERE YEAR(hb.HarvestDate) = @Year AND MONTH(hb.HarvestDate) = @Month
                ) 
                AND f.FarmID NOT IN (
                    SELECT DISTINCT hb.FarmID 
                    FROM OrderDetails od 
                    INNER JOIN HarvestBatches hb ON od.BatchID = hb.BatchID 
                    INNER JOIN Orders o ON od.OrderID = o.OrderID 
                    WHERE YEAR(o.OrderDate) = @Year AND MONTH(o.OrderDate) = @Month
                )", connection);
            command.Parameters.Add(new SqlParameter("@Year", year));
            command.Parameters.Add(new SqlParameter("@Month", month));
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                farms.Add(new Models.Farm
                {
                    FarmID = reader.GetInt32(0),
                    FarmName = reader.GetString(1)
                });
            }
            return farms;
        }

        public async Task<DriverReport?> GetTopDriverAsync(int year, int month)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT TOP 1 d.DriverID, d.DriverName, COUNT(t.TripID) AS TripCount 
                FROM TripToRestaurant t 
                INNER JOIN Driver d ON t.DriverID = d.DriverID 
                WHERE YEAR(t.TripDate) = @Year AND MONTH(t.TripDate) = @Month 
                GROUP BY d.DriverID, d.DriverName 
                ORDER BY TripCount DESC", connection);
            command.Parameters.Add(new SqlParameter("@Year", year));
            command.Parameters.Add(new SqlParameter("@Month", month));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DriverReport
                {
                    DriverID = reader.GetInt32(0),
                    DriverName = reader.GetString(1),
                    TripCount = reader.GetInt32(2)
                };
            }
            return null;
        }

        public async Task<List<Models.Restaurant>> GetRestaurantsWithNoOrdersAsync(int year, int month)
        {
            var restaurants = new List<Models.Restaurant>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT r.RestaurantID, r.RestaurantName, r.Address 
                FROM Restaurants r 
                WHERE r.RestaurantID NOT IN (
                    SELECT DISTINCT o.RestaurantID 
                    FROM Orders o 
                    WHERE YEAR(o.OrderDate) = @Year AND MONTH(o.OrderDate) = @Month
                )", connection);
            command.Parameters.Add(new SqlParameter("@Year", year));
            command.Parameters.Add(new SqlParameter("@Month", month));
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                restaurants.Add(new Models.Restaurant
                {
                    RestaurantID = reader.GetInt32(0),
                    RestaurantName = reader.GetString(1),
                    Address = reader.GetString(2)
                });
            }
            return restaurants;
        }

        public async Task<List<RestaurantBatchReport>> GetBatchesDeliveredToRestaurantsAsync(int year, int month)
        {
            var reports = new List<RestaurantBatchReport>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT r.RestaurantName, hb.BatchID, hb.HarvestDate, hb.Quantity AS BatchQuantity, 
                       c.CropName, f.FarmName, od.Quantity AS OrderedQuantity, od.UnitPrice 
                FROM Orders o 
                INNER JOIN Restaurants r ON o.RestaurantID = r.RestaurantID 
                INNER JOIN OrderDetails od ON od.OrderID = o.OrderID 
                INNER JOIN HarvestBatches hb ON od.BatchID = hb.BatchID 
                INNER JOIN Crops c ON hb.CropID = c.CropID 
                INNER JOIN Farm f ON hb.FarmID = f.FarmID 
                WHERE YEAR(o.OrderDate) = @Year AND MONTH(o.OrderDate) = @Month 
                ORDER BY r.RestaurantName, hb.BatchID", connection);
            command.Parameters.Add(new SqlParameter("@Year", year));
            command.Parameters.Add(new SqlParameter("@Month", month));
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reports.Add(new RestaurantBatchReport
                {
                    RestaurantName = reader.GetString(0),
                    BatchID = reader.GetInt32(1),
                    HarvestDate = reader.GetDateTime(2),
                    BatchQuantity = reader.GetDouble(3),
                    CropName = reader.GetString(4),
                    FarmName = reader.GetString(5),
                    OrderedQuantity = reader.GetDouble(6),
                    UnitPrice = reader.GetDouble(7)
                });
            }
            return reports;
        }
    }
}
