using AISDproject.Data;
using AISDproject.Models;
using AISDproject.Services;
using Microsoft.EntityFrameworkCore;

namespace AISDproject.Services.Implementation
{
    using AISDproject.Data;
    using AISDproject.Models;
    using Microsoft.EntityFrameworkCore;

    public class OrderService : IOrderService
    {
     

        public async Task<IEnumerable<Table>> GetTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }
        private readonly RestaurantContext _context;

        public OrderService(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Staff)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.Staff)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task CreateOrderAsync(Order order, Payment payment)
        {
            order.TotalAmount = order.SubTotal;
            payment.OrderId = order.OrderId;
            payment.Amount = order.TotalAmount;

            using var tx = await _context.Database.BeginTransactionAsync();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}