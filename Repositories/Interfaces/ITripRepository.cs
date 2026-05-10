using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models;

namespace FarmToTable.Repositories.Interfaces
{
    public interface ITripRepository
    {
        Task<List<TripToRestaurant>> GetAllAsync();
        Task<TripToRestaurant?> GetByIdAsync(int id);
        Task CreateAsync(TripToRestaurant trip);
        Task UpdateAsync(TripToRestaurant trip);
        Task DeleteAsync(int id);
    }
}
