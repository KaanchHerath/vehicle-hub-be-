using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly DataContext _context;

        public ReservationService(DataContext context) 
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                throw new DataNotFoundException("Reservation not found");
            }
            return reservation;
        }

        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> UpdateReservation(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            if (existingReservation == null)
            {
               throw new DataNotFoundException("Reservation not found");
            }
            existingReservation.EmployeeId = reservation.EmployeeId;
            existingReservation.Status = reservation.Status;
        
            _context.Entry(existingReservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingReservation;
        }

        public async Task DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                throw new DataNotFoundException("Reservation not found");
            }
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
