using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminReservationServices
{
    public interface IAdminReservationService
    {
        Task AcceptReservation(int id, int eid);
        Task <IEnumerable<ViewReservationDto>> ViewReservations();
        Task BeginReservation(int id);
        Task EndReservation(int id, VehicleLogDto vehicleLog);
    }
}
