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
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
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
    }
}