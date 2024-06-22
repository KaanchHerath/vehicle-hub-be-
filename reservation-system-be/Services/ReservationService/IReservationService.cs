using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.ReservationService
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllReservations();
        Task<Reservation> GetReservation(int id);
        Task<Reservation> CreateReservation(Reservation reservation);
        Task<Reservation> UpdateReservation(int id, Reservation reservation);
        Task DeleteReservation(int id);
    }
}
