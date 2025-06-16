using AISDproject.Data;
using AISDproject.Models;
using Microsoft.EntityFrameworkCore;

namespace AISDproject.Repositeries.Implementation
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RestaurantContext _context;

        public ReservationRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.TimeSlot)
                .ToListAsync();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.TimeSlot)
                .FirstOrDefaultAsync(r => r.ReservationId == id);
        }

        public async Task AddAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);

            // Log kiểm tra
            Console.WriteLine($"📝 Add reservation: {reservation.CustomerName}, {reservation.ReservationDate}");

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            var existing = await _context.Reservations.FindAsync(reservation.ReservationId);
            if (existing != null)
            {
                existing.CustomerName = reservation.CustomerName;
                existing.PhoneNumber = reservation.PhoneNumber;
                existing.ReservationDate = reservation.ReservationDate;
                existing.TableId = reservation.TableId;
                existing.TimeSlotId = reservation.TimeSlotId;
                existing.Status = reservation.Status; // ✅ Quan trọng

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ THÊM MỚI: Lấy danh sách tất cả bàn
        public async Task<List<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        // ✅ THÊM MỚI: Lấy danh sách tất cả khung giờ (TimeSlot)
        public async Task<List<TimeSlot>> GetAllTimeSlotsAsync()
        {
            return await _context.TimeSlots.ToListAsync();
        }


    }
}
