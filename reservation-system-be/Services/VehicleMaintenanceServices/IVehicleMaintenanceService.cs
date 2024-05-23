using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMaintenanceServices
{
    public interface IVehicleMaintenanceService
    {
        Task<IEnumerable<VehicleMaintenanceDto>> GetAllVehicleMaintenances();
        Task<VehicleMaintenanceDto> GetVehicleMaintenance(int id);
        Task<VehicleMaintenance> CreateVehicleMaintenance(VehicleMaintenance vehicleMaintenance);
        Task<VehicleMaintenance> UpdateVehicleMaintenance(int id, VehicleMaintenance vehicleMaintenance);
        Task DeleteVehicleMaintenance(int id);
    }
}
