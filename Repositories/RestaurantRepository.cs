using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly string _connectionString;

        public RestaurantRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Restaurant>> GetAllAsync()
        {
            var restaurants = new List<Restaurant>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT RestaurantID, RestaurantName, Address, DeliveryWindow FROM Restaurants", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                restaurants.Add(new Restaurant
                {
                    RestaurantID = reader.GetInt32(0),
                    RestaurantName = reader.GetString(1),
                    Address = reader.GetString(2),
                    DeliveryWindow = reader.GetTimeSpan(3)
                });
            }
            return restaurants;
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT RestaurantID, RestaurantName, Address, DeliveryWindow FROM Restaurants WHERE RestaurantID = @RestaurantID", connection);
            command.Parameters.Add(new SqlParameter("@RestaurantID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Restaurant
                {
                    RestaurantID = reader.GetInt32(0),
                    RestaurantName = reader.GetString(1),
                    Address = reader.GetString(2),
                    DeliveryWindow = reader.GetTimeSpan(3)
                };
            }
            return null;
        }

        public async Task CreateAsync(Restaurant restaurant)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                INSERT INTO Restaurants (RestaurantName, Address, DeliveryWindow) 
                VALUES (@RestaurantName, @Address, @DeliveryWindow)", connection);
            command.Parameters.Add(new SqlParameter("@RestaurantName", restaurant.RestaurantName));
            command.Parameters.Add(new SqlParameter("@Address", restaurant.Address));
            command.Parameters.Add(new SqlParameter("@DeliveryWindow", restaurant.DeliveryWindow));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Restaurant restaurant)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                UPDATE Restaurants 
                SET RestaurantName = @RestaurantName, Address = @Address, DeliveryWindow = @DeliveryWindow 
                WHERE RestaurantID = @RestaurantID", connection);
            command.Parameters.Add(new SqlParameter("@RestaurantName", restaurant.RestaurantName));
            command.Parameters.Add(new SqlParameter("@Address", restaurant.Address));
            command.Parameters.Add(new SqlParameter("@DeliveryWindow", restaurant.DeliveryWindow));
            command.Parameters.Add(new SqlParameter("@RestaurantID", restaurant.RestaurantID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Restaurants WHERE RestaurantID = @RestaurantID", connection);
            command.Parameters.Add(new SqlParameter("@RestaurantID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
