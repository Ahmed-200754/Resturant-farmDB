using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        private readonly string _connectionString;

        public FarmRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Farm>> GetAllAsync()
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
                    FarmID = reader.GetInt32(0),
                    FarmName = reader.GetString(1),
                    FarmerName = reader.GetString(2),
                    FarmerPhone = reader.GetString(3),
                    Longitude = reader.GetDecimal(4),
                    Latitude = reader.GetDecimal(5)
                });
            }
            return farms;
        }

        public async Task<Farm?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT FarmID, FarmName, FarmerName, FarmerPhone, Longitude, Latitude FROM Farm WHERE FarmID = @FarmID", connection);
            command.Parameters.Add(new SqlParameter("@FarmID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Farm
                {
                    FarmID = reader.GetInt32(0),
                    FarmName = reader.GetString(1),
                    FarmerName = reader.GetString(2),
                    FarmerPhone = reader.GetString(3),
                    Longitude = reader.GetDecimal(4),
                    Latitude = reader.GetDecimal(5)
                };
            }
            return null;
        }

        public async Task CreateAsync(Farm farm)
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

        public async Task UpdateAsync(Farm farm)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE Farm SET FarmName = @FarmName, FarmerName = @FarmerName, FarmerPhone = @FarmerPhone, Longitude = @Longitude, Latitude = @Latitude WHERE FarmID = @FarmID", connection);
            command.Parameters.Add(new SqlParameter("@FarmName", farm.FarmName));
            command.Parameters.Add(new SqlParameter("@FarmerName", farm.FarmerName));
            command.Parameters.Add(new SqlParameter("@FarmerPhone", farm.FarmerPhone));
            command.Parameters.Add(new SqlParameter("@Longitude", farm.Longitude));
            command.Parameters.Add(new SqlParameter("@Latitude", farm.Latitude));
            command.Parameters.Add(new SqlParameter("@FarmID", farm.FarmID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Farm WHERE FarmID = @FarmID", connection);
            command.Parameters.Add(new SqlParameter("@FarmID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
