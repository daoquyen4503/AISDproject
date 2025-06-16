using AISDproject.Data;
using AISDproject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AISDproject.Services.Implementation
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly RestaurantContext _context;

        public TimeSlotService(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<List<TimeSlot>> GetAllTimeSlotsAsync() // ✅ Giữ đúng tên như interface
        {
            return await _context.TimeSlots.ToListAsync();
        }

        public async Task<TimeSlot?> GetByIdAsync(int id)
            => await _context.TimeSlots.FindAsync(id);

        public async Task CreateAsync(TimeSlot timeSlot)
        {
            _context.TimeSlots.Add(timeSlot);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TimeSlot timeSlot)
        {
            _context.TimeSlots.Update(timeSlot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slot = await _context.TimeSlots.FindAsync(id);
            if (slot != null)
            {
                _context.TimeSlots.Remove(slot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.TimeSlots.AnyAsync(t => t.TimeSlotId == id);
    }
}
