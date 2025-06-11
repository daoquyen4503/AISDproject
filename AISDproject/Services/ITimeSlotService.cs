using AISDproject.Models;

namespace AISDproject.Services
{
    public interface ITimeSlotService
    {
        Task<IEnumerable<TimeSlot>> GetAllAsync();
        Task<TimeSlot?> GetByIdAsync(int id);
        Task CreateAsync(TimeSlot timeSlot);
        Task UpdateAsync(TimeSlot timeSlot);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

}
