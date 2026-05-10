using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models.ViewModels;

namespace FarmToTable.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<CropReport?> GetHighestOrderedCropAsync();
        Task<List<FarmRevenue>> GetTopFarmsByRevenueAsync();
        Task<List<FarmRevenue>> GetTotalRevenuePerFarmAsync();
        Task<List<Models.Farm>> GetInactiveFarmsAsync(int year, int month);
        Task<DriverReport?> GetTopDriverAsync(int year, int month);
        Task<List<Models.Restaurant>> GetRestaurantsWithNoOrdersAsync(int year, int month);
        Task<List<RestaurantBatchReport>> GetBatchesDeliveredToRestaurantsAsync(int year, int month);
    }
}
