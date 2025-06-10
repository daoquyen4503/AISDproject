using AISDproject.Models;
using Microsoft.EntityFrameworkCore;

namespace AISDproject.Data
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }

    }
}
