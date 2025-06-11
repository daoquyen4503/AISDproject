using AISDproject.Data;
using AISDproject.Models;
using Microsoft.EntityFrameworkCore;

namespace AISDproject.Repositeries.Implementation
{
    public class StaffRepository : IStaffRepository
    {
        private readonly RestaurantContext _context;

        public StaffRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Staff>> GetAllAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<Staff> GetByIdAsync(int id)
        {
            return await _context.Staffs.FirstOrDefaultAsync(s => s.StaffId == id);
        }

        public async Task AddAsync(Staff staff)
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Staff staff)
        {
            _context.Staffs.Update(staff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();
            }
        }
    }
}
