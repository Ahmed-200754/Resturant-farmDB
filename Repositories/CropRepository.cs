using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class CropRepository : ICropRepository
    {
        private readonly string _connectionString;

        public CropRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<Crop>> GetAllAsync()
        {
            var crops = new List<Crop>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT c.CropID, c.CropName, 
                       (SELECT COUNT(*) FROM HarvestBatches hb WHERE hb.CropID = c.CropID) as BatchCount
                FROM Crops c", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                crops.Add(new Crop
                {
                    CropID = reader.GetInt32(0),
                    CropName = reader.GetString(1),
                    BatchCount = reader.GetInt32(2)
                });
            }
            return crops;
        }

        public async Task<Crop?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT CropID, CropName FROM Crops WHERE CropID = @CropID", connection);
            command.Parameters.Add(new SqlParameter("@CropID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Crop
                {
                    CropID = reader.GetInt32(0),
                    CropName = reader.GetString(1)
                };
            }
            return null;
        }

        public async Task CreateAsync(Crop crop)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("INSERT INTO Crops (CropName) VALUES (@CropName)", connection);
            command.Parameters.Add(new SqlParameter("@CropName", crop.CropName));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Crop crop)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("UPDATE Crops SET CropName = @CropName WHERE CropID = @CropID", connection);
            command.Parameters.Add(new SqlParameter("@CropName", crop.CropName));
            command.Parameters.Add(new SqlParameter("@CropID", crop.CropID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM Crops WHERE CropID = @CropID", connection);
            command.Parameters.Add(new SqlParameter("@CropID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
