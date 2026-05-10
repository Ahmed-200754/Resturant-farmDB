using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models;

namespace FarmToTable.Repositories.Interfaces
{
    public interface ITaskRequirementsRepository
    {
        // 1. INSERT STATEMENTS
        Task InsertNewFarmAsync(Farm farm);
        Task InsertNewRestaurantAsync(Restaurant restaurant);
        Task InsertNewHarvestBatchAsync(HarvestBatch batch);

        // 2. DELETE STATEMENTS
        Task DeleteHarvestBatchAsync(int batchId);
        Task DeleteRestaurantAsync(int restaurantId);
        Task DeleteDriverAsync(int driverId);

        // 3. UPDATE STATEMENTS
        Task UpdateFarmPhoneNumberAsync(int farmId, string newPhoneNumber);
        Task UpdateRestaurantDeliveryWindowAsync(int restaurantId, TimeSpan newWindow);
        Task UpdateHarvestBatchQuantityAsync(int batchId, double newQuantity);

        // 4. SIMPLE SELECT QUERIES
        Task<List<Farm>> GetAllFarmsAsync();
        Task<List<Restaurant>> GetAllRestaurantsAsync();
        Task<List<HarvestBatch>> GetAllHarvestBatchesAsync();

        // 5. JOIN QUERIES
        Task<List<Dictionary<string, object>>> GetRestaurantOrdersWithBatchDetailsAsync();
        Task<List<Dictionary<string, object>>> GetCropNamesWithHarvestBatchesAsync();
        Task<List<Dictionary<string, object>>> GetFarmBatchesWithCropInfoAsync();
        Task<List<Dictionary<string, object>>> GetOrdersWithRestaurantNamesAsync();
    }
}
