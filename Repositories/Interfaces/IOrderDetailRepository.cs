using System.Collections.Generic;
using System.Threading.Tasks;
using FarmToTable.Models;

namespace FarmToTable.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetAllByOrderIdAsync(int orderId);
        Task<OrderDetail?> GetByIdAsync(int id);
        Task CreateAsync(OrderDetail detail);
        Task UpdateAsync(OrderDetail detail);
        Task DeleteAsync(int id);
    }
}
