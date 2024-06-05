using reservation_system_be.DTOs;

namespace reservation_system_be.Services.ReservationStatusServices
{
    public interface IReservationStatusService
    {
        public Task<ReservationStatusDTO> GetAllReservationStatus();
    }
}
