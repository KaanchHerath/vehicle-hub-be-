using reservation_system_be.Models;

namespace reservation_system_be.Services.MaintenanceTypeServices
{
    public interface IMaintenanceTypeService
    {
        Task<List<MaintenanceType>> GetAllMaintenanceType();
        Task<MaintenanceType?> GetSingleMaintenanceType(int id);
        Task<List<MaintenanceType>> AddMaintenanceType(MaintenanceType maintenanceType);
        Task<List<MaintenanceType>?> UpdateMaintenanceType(int id, MaintenanceType request);
        Task<List<MaintenanceType>?> DeleteMaintenanceType(int id);
    }
}
