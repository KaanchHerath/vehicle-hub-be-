using reservation_system_be.DTOs;

namespace reservation_system_be.Services.DashboardStatusServices
{
    public interface IDashboardStatusService
    {
        public Task<DashboardStatusDTO> GetAllDashboardStatus();
    }
}
