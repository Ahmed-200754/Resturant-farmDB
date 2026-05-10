using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class TaskRequirementsRepository : ITaskRequirementsRepository
    {
        private readonly string _connectionString;

        public TaskRequirementsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task InsertNewFarmAsync(Farm farm)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO Farm (FarmName, FarmerName, FarmerPhone, Longitude, Latitude) VALUES (@FarmName, @FarmerName, @FarmerPhone, @Longitude, @Latitude)", connection);
            command.Parameters.Add(new SqlParameter("@FarmName", farm.FarmName));
            command.Parameters.Add(new SqlParameter("@FarmerName", farm.FarmerName));
            command.Parameters.Add(new SqlParameter("@FarmerPhone", farm.FarmerPhone));
            command.Parameters.Add(new SqlParameter("@Longitude", farm.Longitude));
            command.Parameters.Add(new SqlParameter("@Latitude", farm.Latitude));
            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertNewRestaurantAsync(Restaurant restaurant)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO Restaurants (RestaurantName, Address, DeliveryWindow) VALUES (@RestaurantName, @Address, @DeliveryWindow)", connection);
            command.Parameters.Add(new SqlParameter("@RestaurantName", restaurant.RestaurantName));
            command.Parameters.Add(new SqlParameter("@Address", restaurant.Address));
            command.Parameters.Add(new SqlParameter("@DeliveryWindow", restaurant.DeliveryWindow));
            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertNewHarvestBatchAsync(HarvestBatch batch)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO HarvestBatches (HarvestDate, Quantity, FreshnessWindow, CropID, FarmID) VALUES (@HarvestDate, @Quantity, @FreshnessWindow, @CropID, @FarmID)", connection);
            command.Parameters.Add(new SqlParameter("@HarvestDate", batch.HarvestDate));
            command.Parameters.Add(new SqlParameter("@Quantity", batch.Quantity));
            command.Parameters.Add(new SqlParameter("@FreshnessWindow", batch.FreshnessWindow));
            command.Parameters.Add(new SqlParameter("@CropID", batch.CropID));
            command.Parameters.Add(new SqlParameter("@FarmID", batch.FarmID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteHarvestBatchAsync(int batchId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM HarvestBatches WHERE BatchID = @BatchID", connection);
            command.Parameters.Add(new SqlParameter("@BatchID", batchId));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteRestaurantAsync(int restaurantId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Restaurants WHERE RestaurantID = @RestaurantID", connection);
            command.Parameters.Add(new SqlParameter("@RestaurantID", restaurantId));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteDriverAsync(int driverId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Driver WHERE DriverID = @DriverID", connection);
            command.Parameters.Add(new SqlParameter("@DriverID", driverId));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateFarmPhoneNumberAsync(int farmId, string newPhoneNumber)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE Farm SET FarmerPhone = @Phone WHERE FarmID = @FarmID", connection);
            command.Parameters.Add(new SqlParameter("@Phone", newPhoneNumber));
            command.Parameters.Add(new SqlParameter("@FarmID", farmId));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateRestaurantDeliveryWindowAsync(int restaurantId, TimeSpan newWindow)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE Restaurants SET DeliveryWindow = @Window WHERE RestaurantID = @RestaurantID", connection);
            command.Parameters.Add(new SqlParameter("@Window", newWindow));
            command.Parameters.Add(new SqlParameter("@RestaurantID", restaurantId));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateHarvestBatchQuantityAsync(int batchId, double newQuantity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE HarvestBatches SET Quantity = @Quantity WHERE BatchID = @BatchID", connection);
            command.Parameters.Add(new SqlParameter("@Quantity", newQuantity));
            command.Parameters.Add(new SqlParameter("@BatchID", batchId));
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Farm>> GetAllFarmsAsync()
        {
            var farms = new List<Farm>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT FarmID, FarmName, FarmerName, FarmerPhone, Longitude, Latitude FROM Farm", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                farms.Add(new Farm
                {
                    FarmID = reader.GetInt32(0), FarmName = reader.GetString(1), FarmerName = reader.GetString(2),
                    FarmerPhone = reader.GetString(3), Longitude = reader.GetDecimal(4), Latitude = reader.GetDecimal(5)
                });
            }
            return farms;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var results = new List<Restaurant>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT RestaurantID, RestaurantName, Address, DeliveryWindow FROM Restaurants", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new Restaurant
                {
                    RestaurantID = reader.GetInt32(0), RestaurantName = reader.GetString(1),
                    Address = reader.GetString(2), DeliveryWindow = reader.GetTimeSpan(3)
                });
            }
            return results;
        }

        public async Task<List<HarvestBatch>> GetAllHarvestBatchesAsync()
        {
            var batches = new List<HarvestBatch>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT BatchID, HarvestDate, Quantity, FreshnessWindow, CropID, FarmID FROM HarvestBatches", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                batches.Add(new HarvestBatch
                {
                    BatchID = reader.GetInt32(0), HarvestDate = reader.GetDateTime(1),
                    Quantity = reader.GetDouble(2), FreshnessWindow = reader.GetDateTime(3),
                    CropID = reader.GetInt32(4), FarmID = reader.GetInt32(5)
                });
            }
            return batches;
        }

        public async Task<List<Dictionary<string, object>>> GetRestaurantOrdersWithBatchDetailsAsync()
        {
            var results = new List<Dictionary<string, object>>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT r.RestaurantName, o.OrderID, o.OrderDate, od.Quantity, od.UnitPrice, hb.BatchID
                FROM Restaurants r
                INNER JOIN Orders o ON r.RestaurantID = o.RestaurantID
                INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
                INNER JOIN HarvestBatches hb ON od.BatchID = hb.BatchID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new Dictionary<string, object> {
                    {"RestaurantName", reader.GetString(0)}, {"OrderID", reader.GetInt32(1)},
                    {"OrderDate", reader.GetDateTime(2)}, {"Quantity", reader.GetDouble(3)},
                    {"UnitPrice", reader.GetDouble(4)}, {"BatchID", reader.GetInt32(5)}
                });
            }
            return results;
        }

        public async Task<List<Dictionary<string, object>>> GetCropNamesWithHarvestBatchesAsync()
        {
            var results = new List<Dictionary<string, object>>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT c.CropName, hb.BatchID, hb.HarvestDate, hb.Quantity
                FROM Crops c INNER JOIN HarvestBatches hb ON c.CropID = hb.CropID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new Dictionary<string, object> {
                    {"CropName", reader.GetString(0)}, {"BatchID", reader.GetInt32(1)},
                    {"HarvestDate", reader.GetDateTime(2)}, {"Quantity", reader.GetDouble(3)}
                });
            }
            return results;
        }

        public async Task<List<Dictionary<string, object>>> GetFarmBatchesWithCropInfoAsync()
        {
            var results = new List<Dictionary<string, object>>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT f.FarmName, hb.BatchID, hb.Quantity, c.CropName
                FROM Farm f INNER JOIN HarvestBatches hb ON f.FarmID = hb.FarmID
                INNER JOIN Crops c ON hb.CropID = c.CropID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new Dictionary<string, object> {
                    {"FarmName", reader.GetString(0)}, {"BatchID", reader.GetInt32(1)},
                    {"Quantity", reader.GetDouble(2)}, {"CropName", reader.GetString(3)}
                });
            }
            return results;
        }

        public async Task<List<Dictionary<string, object>>> GetOrdersWithRestaurantNamesAsync()
        {
            var results = new List<Dictionary<string, object>>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT o.OrderID, o.OrderDate, r.RestaurantName
                FROM Orders o INNER JOIN Restaurants r ON o.RestaurantID = r.RestaurantID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new Dictionary<string, object> {
                    {"OrderID", reader.GetInt32(0)}, {"OrderDate", reader.GetDateTime(1)},
                    {"RestaurantName", reader.GetString(2)}
                });
            }
            return results;
        }
    }
}
