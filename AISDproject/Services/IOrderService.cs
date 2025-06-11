using System.Collections.Generic;
using System.Threading.Tasks;
using AISDproject.Models;

namespace AISDproject.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order, Payment payment);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);

        // Dropdown support (async)
        Task<IEnumerable<Table>> GetTablesAsync();
        Task<IEnumerable<Staff>> GetStaffsAsync();
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
    }
}