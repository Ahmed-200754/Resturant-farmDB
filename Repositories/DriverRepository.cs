using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly string _connectionString;

        public DriverRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Driver>> GetAllAsync()
        {
            var drivers = new List<Driver>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT DriverID, DriverName, DriverDOB, DriverPhone FROM Driver", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                drivers.Add(new Driver
                {
                    DriverID = reader.GetInt32(0),
                    DriverName = reader.GetString(1),
                    DriverDOB = reader.GetDateTime(2),
                    DriverPhone = reader.GetString(3)
                });
            }
            return drivers;
        }

        public async Task<Driver?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT DriverID, DriverName, DriverDOB, DriverPhone FROM Driver WHERE DriverID = @DriverID", connection);
            command.Parameters.Add(new SqlParameter("@DriverID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Driver
                {
                    DriverID = reader.GetInt32(0),
                    DriverName = reader.GetString(1),
                    DriverDOB = reader.GetDateTime(2),
                    DriverPhone = reader.GetString(3)
                };
            }
            return null;
        }

        public async Task CreateAsync(Driver driver)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                INSERT INTO Driver (DriverName, DriverDOB, DriverPhone) 
                VALUES (@DriverName, @DriverDOB, @DriverPhone)", connection);
            command.Parameters.Add(new SqlParameter("@DriverName", driver.DriverName));
            command.Parameters.Add(new SqlParameter("@DriverDOB", driver.DriverDOB.Date));
            command.Parameters.Add(new SqlParameter("@DriverPhone", driver.DriverPhone));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                UPDATE Driver 
                SET DriverName = @DriverName, DriverDOB = @DriverDOB, DriverPhone = @DriverPhone 
                WHERE DriverID = @DriverID", connection);
            command.Parameters.Add(new SqlParameter("@DriverName", driver.DriverName));
            command.Parameters.Add(new SqlParameter("@DriverDOB", driver.DriverDOB.Date));
            command.Parameters.Add(new SqlParameter("@DriverPhone", driver.DriverPhone));
            command.Parameters.Add(new SqlParameter("@DriverID", driver.DriverID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Driver WHERE DriverID = @DriverID", connection);
            command.Parameters.Add(new SqlParameter("@DriverID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
