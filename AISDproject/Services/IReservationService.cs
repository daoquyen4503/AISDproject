using AISDproject.Models;

namespace AISDproject.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task CreateReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(int id);
        Task<List<Table>> GetAllTablesAsync();
        Task<List<TimeSlot>> GetAllTimeSlotsAsync();


    }
}
