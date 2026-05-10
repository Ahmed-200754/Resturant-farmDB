using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models;

namespace FarmToTable.Repositories.Interfaces
{
    public interface IHarvestBatchRepository
    {
        Task<List<HarvestBatch>> GetAllAsync();
        Task<HarvestBatch?> GetByIdAsync(int id);
        Task CreateAsync(HarvestBatch batch);
        Task UpdateAsync(HarvestBatch batch);
        Task DeleteAsync(int id);
    }
}
