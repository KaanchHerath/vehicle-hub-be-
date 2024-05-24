using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminPanelServices
{
    public interface IAdminPanelService
    {
        Task AcceptReservation(int id, int eid);
        Task <IEnumerable<ViewReservationDto>> ViewReservations();
    }
}
