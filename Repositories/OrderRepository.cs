using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT o.OrderID, o.OrderDate, o.RestaurantID, r.RestaurantName,
                       (SELECT COUNT(*) FROM OrderDetails od WHERE od.OrderID = o.OrderID) AS TotalItems,
                       COALESCE((SELECT SUM(od.UnitPrice * od.Quantity) FROM OrderDetails od WHERE od.OrderID = o.OrderID), 0) AS TotalValue
                FROM Orders o
                INNER JOIN Restaurants r ON o.RestaurantID = r.RestaurantID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order
                {
                    OrderID = reader.GetInt32(0),
                    OrderDate = reader.GetDateTime(1),
                    RestaurantID = reader.GetInt32(2),
                    RestaurantName = reader.GetString(3),
                    TotalItems = reader.GetInt32(4),
                    TotalValue = reader.GetDouble(5)
                });
            }
            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT OrderID, OrderDate, RestaurantID FROM Orders WHERE OrderID = @OrderID", connection);
            command.Parameters.Add(new SqlParameter("@OrderID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Order
                {
                    OrderID = reader.GetInt32(0),
                    OrderDate = reader.GetDateTime(1),
                    RestaurantID = reader.GetInt32(2)
                };
            }
            return null;
        }

        public async Task CreateAsync(Order order)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO Orders (OrderDate, RestaurantID) VALUES (@OrderDate, @RestaurantID)", connection);
            command.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate.Date));
            command.Parameters.Add(new SqlParameter("@RestaurantID", order.RestaurantID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE Orders SET OrderDate = @OrderDate, RestaurantID = @RestaurantID WHERE OrderID = @OrderID", connection);
            command.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate.Date));
            command.Parameters.Add(new SqlParameter("@RestaurantID", order.RestaurantID));
            command.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            // Need to delete details first or let cascade handle it, assuming no cascade in requirements, we delete details here or rely on explicit details deletion.
            using var detailsCommand = new SqlCommand("DELETE FROM OrderDetails WHERE OrderID = @OrderID", connection);
            detailsCommand.Parameters.Add(new SqlParameter("@OrderID", id));
            await detailsCommand.ExecuteNonQueryAsync();

            using var command = new SqlCommand("DELETE FROM Orders WHERE OrderID = @OrderID", connection);
            command.Parameters.Add(new SqlParameter("@OrderID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
