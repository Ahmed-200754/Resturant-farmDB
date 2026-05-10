using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models;

namespace FarmToTable.Repositories.Interfaces
{
    public interface IFarmRepository
    {
        Task<List<Farm>> GetAllAsync();
        Task<Farm?> GetByIdAsync(int id);
        Task CreateAsync(Farm farm);
        Task UpdateAsync(Farm farm);
        Task DeleteAsync(int id);
    }
}
