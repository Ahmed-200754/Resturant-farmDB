using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models;

namespace FarmToTable.Repositories.Interfaces
{
    public interface ICropRepository
    {
        Task<List<Crop>> GetAllAsync();
        Task<Crop?> GetByIdAsync(int id);
        Task CreateAsync(Crop crop);
        Task UpdateAsync(Crop crop);
        Task DeleteAsync(int id);
    }
}
