using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FarmToTable.Models;
using FarmToTable.Repositories.Interfaces;

namespace FarmToTable.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly string _connectionString;

        public TripRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<TripToRestaurant>> GetAllAsync()
        {
            var trips = new List<TripToRestaurant>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                SELECT t.TripID, t.TripDate, t.Distance, t.RouteTaken, t.DriverID, t.FarmID,
                       d.DriverName, f.FarmName
                FROM TripToRestaurant t
                INNER JOIN Driver d ON t.DriverID = d.DriverID
                INNER JOIN Farm f ON t.FarmID = f.FarmID", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                trips.Add(new TripToRestaurant
                {
                    TripID = reader.GetInt32(0),
                    TripDate = reader.GetDateTime(1),
                    Distance = reader.GetInt32(2),
                    RouteTaken = reader.GetString(3),
                    DriverID = reader.GetInt32(4),
                    FarmID = reader.GetInt32(5),
                    DriverName = reader.GetString(6),
                    FarmName = reader.GetString(7)
                });
            }
            return trips;
        }

        public async Task<TripToRestaurant?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT TripID, TripDate, Distance, RouteTaken, DriverID, FarmID FROM TripToRestaurant WHERE TripID = @TripID", connection);
            command.Parameters.Add(new SqlParameter("@TripID", id));
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new TripToRestaurant
                {
                    TripID = reader.GetInt32(0),
                    TripDate = reader.GetDateTime(1),
                    Distance = reader.GetInt32(2),
                    RouteTaken = reader.GetString(3),
                    DriverID = reader.GetInt32(4),
                    FarmID = reader.GetInt32(5)
                };
            }
            return null;
        }

        public async Task CreateAsync(TripToRestaurant trip)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                INSERT INTO TripToRestaurant (TripDate, Distance, RouteTaken, DriverID, FarmID) 
                VALUES (@TripDate, @Distance, @RouteTaken, @DriverID, @FarmID)", connection);
            command.Parameters.Add(new SqlParameter("@TripDate", trip.TripDate.Date));
            command.Parameters.Add(new SqlParameter("@Distance", trip.Distance));
            command.Parameters.Add(new SqlParameter("@RouteTaken", trip.RouteTaken));
            command.Parameters.Add(new SqlParameter("@DriverID", trip.DriverID));
            command.Parameters.Add(new SqlParameter("@FarmID", trip.FarmID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(TripToRestaurant trip)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand(@"
                UPDATE TripToRestaurant 
                SET TripDate = @TripDate, Distance = @Distance, RouteTaken = @RouteTaken, 
                    DriverID = @DriverID, FarmID = @FarmID 
                WHERE TripID = @TripID", connection);
            command.Parameters.Add(new SqlParameter("@TripDate", trip.TripDate.Date));
            command.Parameters.Add(new SqlParameter("@Distance", trip.Distance));
            command.Parameters.Add(new SqlParameter("@RouteTaken", trip.RouteTaken));
            command.Parameters.Add(new SqlParameter("@DriverID", trip.DriverID));
            command.Parameters.Add(new SqlParameter("@FarmID", trip.FarmID));
            command.Parameters.Add(new SqlParameter("@TripID", trip.TripID));
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand("DELETE FROM TripToRestaurant WHERE TripID = @TripID", connection);
            command.Parameters.Add(new SqlParameter("@TripID", id));
            await command.ExecuteNonQueryAsync();
        }
    }
}
