using AISDproject.Models;

namespace AISDproject.Repositeries
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int id);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(int id);
        Task<List<TimeSlot>> GetAllTimeSlotsAsync();

    }

}
