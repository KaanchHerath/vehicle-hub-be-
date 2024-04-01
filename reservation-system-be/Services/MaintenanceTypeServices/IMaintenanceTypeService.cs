using reservation_system_be.Models;

namespace reservation_system_be.Services.MaintenanceTypeServices
{
    public interface IMaintenanceTypeService
    {
        Task<List<MaintenanceType>> GetAllMaintenanceType();
        Task<VehicleMake?> GetSingleMaintenanceType(int id);
        Task<List<VehicleMake>> AddMaintenanceType(MaintenanceType maintenanceType);
        Task<List<VehicleMake>?> UpdateMaintenanceType(int id, MaintenanceType request);
        Task<List<VehicleMake>?> DeleteMaintenanceType(int id);
    }
}
