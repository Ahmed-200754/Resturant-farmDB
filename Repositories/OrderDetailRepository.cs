using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly string _connectionString;

        public OrderDetailRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<OrderDetail>> GetAllByOrderIdAsync(int orderId)
        {
            var details = new List<OrderDetail>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT od.OrderDetailID, od.UnitPrice, od.Quantity, od.OrderID, od.BatchID,
                       c.CropName, f.FarmName
                FROM OrderDetails od
                INNER JOIN HarvestBatches hb ON od.BatchID = hb.BatchID
                INNER JOIN Crops c ON hb.CropID = c.CropID
                INNER JOIN Farm f ON hb.FarmID = f.FarmID
                WHERE od.OrderID = @OrderID", connection);
            command.Parameters.Add(new SqlParameter("@OrderID", orderId));
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                details.Add(new OrderDetail
                {
                    OrderDetailID = reader.GetInt32(0),
                    UnitPrice = reader.GetDouble(1),
                    Quantity = reader.GetDouble(2),
                    OrderID = reader.GetInt32(3),
                    BatchID = reader.GetInt32(4),
                    CropName = reader.GetString(5),
                    FarmName = reader.GetString(6)
                });
            }
            return details;
        }

        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT OrderDetailID, UnitPrice, Quantity, OrderID, BatchID FROM OrderDetails WHERE OrderDetailID = @OrderDetailID", connection);
            command.Parameters.Add(new SqlParameter("@OrderDetailID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new OrderDetail
                {
                    OrderDetailID = reader.GetInt32(0),
                    UnitPrice = reader.GetDouble(1),
                    Quantity = reader.GetDouble(2),
                    OrderID = reader.GetInt32(3),
                    BatchID = reader.GetInt32(4)
                };
            }
            return null;
        }

        public async Task CreateAsync(OrderDetail detail)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO OrderDetails (UnitPrice, Quantity, OrderID, BatchID) VALUES (@UnitPrice, @Quantity, @OrderID, @BatchID)", connection);
            command.Parameters.Add(new SqlParameter("@UnitPrice", detail.UnitPrice));
            command.Parameters.Add(new SqlParameter("@Quantity", detail.Quantity));
            command.Parameters.Add(new SqlParameter("@OrderID", detail.OrderID));
            command.Parameters.Add(new SqlParameter("@BatchID", detail.BatchID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(OrderDetail detail)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE OrderDetails SET UnitPrice = @UnitPrice, Quantity = @Quantity, BatchID = @BatchID WHERE OrderDetailID = @OrderDetailID", connection);
            command.Parameters.Add(new SqlParameter("@UnitPrice", detail.UnitPrice));
            command.Parameters.Add(new SqlParameter("@Quantity", detail.Quantity));
            command.Parameters.Add(new SqlParameter("@BatchID", detail.BatchID));
            command.Parameters.Add(new SqlParameter("@OrderDetailID", detail.OrderDetailID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM OrderDetails WHERE OrderDetailID = @OrderDetailID", connection);
            command.Parameters.Add(new SqlParameter("@OrderDetailID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
