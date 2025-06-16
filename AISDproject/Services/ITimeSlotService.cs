using AISDproject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AISDproject.Services
{
    public interface ITimeSlotService
    {
        Task<List<TimeSlot>> GetAllTimeSlotsAsync(); // ✅ Giữ nguyên tên này
        Task<TimeSlot?> GetByIdAsync(int id);
        Task CreateAsync(TimeSlot timeSlot);
        Task UpdateAsync(TimeSlot timeSlot);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
