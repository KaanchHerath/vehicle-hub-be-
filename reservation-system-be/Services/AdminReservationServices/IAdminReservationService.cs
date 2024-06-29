using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminReservationServices
{
    public interface IAdminReservationService
    {
        Task AcceptReservation(int id, int eid);
        Task DeclineReservation(int id, int eid);
        Task <IEnumerable<ViewReservationDto>> ViewReservations();
        Task BeginReservation(int id, int eid);
        Task EndReservation(int id, int eid, VehicleLogDto vehicleLog);
        Task <IEnumerable<VehicleCardDto>> AvailableVehicles(int id);
        Task ReservationChangeVehicle(int id, int vid);
        Task CancelReservation(int id, int eid);
        Task<CustomerHoverDto> CustomerDetails(int id);
        Task<VehicleLogDescriptionHoverDto> GetVehicleLogDescription(int id);
    }
}
