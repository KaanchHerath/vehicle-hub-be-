using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.ReservationStatusServices
{
    public class ReservationStatusService : IReservationStatusService
    {
        private readonly DataContext _context;

        public ReservationStatusService(DataContext context)
        {
            _context = context;
        }

        public async Task<ReservationStatusDTO> GetAllReservationStatus()
        {
            try
            {
                var counts = await _context.Reservations
                    .GroupBy(r => r.Status)
                    .Select(g => new
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var result = new ReservationStatusDTO
                {
                    ConfirmedCount = counts.FirstOrDefault(c => c.Status == Status.Completed)?.Count ?? 0,
                    PendingCount = counts.FirstOrDefault(c => c.Status == Status.Pending)?.Count ?? 0,
                    CancelledCount = counts.FirstOrDefault(c => c.Status == Status.Cancelled)?.Count ?? 0
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting reservation counts", ex);
            }
        }
    }
}
