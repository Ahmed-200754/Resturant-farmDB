using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class HarvestBatchRepository : IHarvestBatchRepository
    {
        private readonly string _connectionString;

        public HarvestBatchRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<HarvestBatch>> GetAllAsync()
        {
            var batches = new List<HarvestBatch>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT hb.BatchID, hb.HarvestDate, hb.Quantity, hb.FreshnessWindow, hb.CropID, hb.FarmID,
                       c.CropName, f.FarmName
                FROM HarvestBatches hb
                INNER JOIN Crops c ON hb.CropID = c.CropID
                INNER JOIN Farm f ON hb.FarmID = f.FarmID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                batches.Add(new HarvestBatch
                {
                    BatchID = reader.GetInt32(0),
                    HarvestDate = reader.GetDateTime(1),
                    Quantity = reader.GetDouble(2),
                    FreshnessWindow = reader.GetDateTime(3),
                    CropID = reader.GetInt32(4),
                    FarmID = reader.GetInt32(5),
                    CropName = reader.GetString(6),
                    FarmName = reader.GetString(7)
                });
            }
            return batches;
        }

        public async Task<HarvestBatch?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT BatchID, HarvestDate, Quantity, FreshnessWindow, CropID, FarmID
                FROM HarvestBatches 
                WHERE BatchID = @BatchID", connection);
            command.Parameters.Add(new SqlParameter("@BatchID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new HarvestBatch
                {
                    BatchID = reader.GetInt32(0),
                    HarvestDate = reader.GetDateTime(1),
                    Quantity = reader.GetDouble(2),
                    FreshnessWindow = reader.GetDateTime(3),
                    CropID = reader.GetInt32(4),
                    FarmID = reader.GetInt32(5)
                };
            }
            return null;
        }

        public async Task CreateAsync(HarvestBatch batch)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                INSERT INTO HarvestBatches (HarvestDate, Quantity, FreshnessWindow, CropID, FarmID) 
                VALUES (@HarvestDate, @Quantity, @FreshnessWindow, @CropID, @FarmID)", connection);
            command.Parameters.Add(new SqlParameter("@HarvestDate", batch.HarvestDate.Date));
            command.Parameters.Add(new SqlParameter("@Quantity", batch.Quantity));
            command.Parameters.Add(new SqlParameter("@FreshnessWindow", batch.FreshnessWindow.Date));
            command.Parameters.Add(new SqlParameter("@CropID", batch.CropID));
            command.Parameters.Add(new SqlParameter("@FarmID", batch.FarmID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(HarvestBatch batch)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                UPDATE HarvestBatches 
                SET HarvestDate = @HarvestDate, Quantity = @Quantity, FreshnessWindow = @FreshnessWindow, 
                    CropID = @CropID, FarmID = @FarmID 
                WHERE BatchID = @BatchID", connection);
            command.Parameters.Add(new SqlParameter("@HarvestDate", batch.HarvestDate.Date));
            command.Parameters.Add(new SqlParameter("@Quantity", batch.Quantity));
            command.Parameters.Add(new SqlParameter("@FreshnessWindow", batch.FreshnessWindow.Date));
            command.Parameters.Add(new SqlParameter("@CropID", batch.CropID));
            command.Parameters.Add(new SqlParameter("@FarmID", batch.FarmID));
            command.Parameters.Add(new SqlParameter("@BatchID", batch.BatchID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            
            // Delete dependent records first to prevent Foreign Key constraint violation
            using var deleteDetailsCommand = new SqlCommand("DELETE FROM OrderDetails WHERE BatchID = @BatchID", connection);
            deleteDetailsCommand.Parameters.Add(new SqlParameter("@BatchID", id));
            await deleteDetailsCommand.ExecuteNonQueryAsync();

            // Now safely delete the harvest batch
            using var command = new SqlCommand("DELETE FROM HarvestBatches WHERE BatchID = @BatchID", connection);
            command.Parameters.Add(new SqlParameter("@BatchID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
