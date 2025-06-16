using AISDproject.Models;
using AISDproject.Repositeries;
using AISDproject.Repositeries.Implementation;
using Microsoft.EntityFrameworkCore;

namespace AISDproject.Services.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateReservationAsync(Reservation reservation)
        {
            await _repository.AddAsync(reservation);
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            await _repository.UpdateAsync(reservation);
        }

        public async Task DeleteReservationAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        // ✅ THÊM MỚI: Lấy danh sách tất cả bàn
        public async Task<List<Table>> GetAllTablesAsync()
        {
            // Đảm bảo _repository là kiểu cụ thể có method GetAllTablesAsync
            if (_repository is ReservationRepository concreteRepo)
            {
                return await concreteRepo.GetAllTablesAsync();
            }

            throw new InvalidOperationException("Repository không hỗ trợ GetAllTablesAsync");
        }

        public async Task<List<TimeSlot>> GetAllTimeSlotsAsync()
        {
            if (_repository is ReservationRepository concreteRepo)
            {
                return await concreteRepo.GetAllTimeSlotsAsync();
            }

            throw new InvalidOperationException("Repository không hỗ trợ GetAllTimeSlotsAsync");
        }


    }
}
