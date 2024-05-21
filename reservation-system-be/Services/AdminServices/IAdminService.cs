using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminServices
{
    public interface IAdminService
    {
        Task AcceptReservation(int id, Employee employee);
    }
}
