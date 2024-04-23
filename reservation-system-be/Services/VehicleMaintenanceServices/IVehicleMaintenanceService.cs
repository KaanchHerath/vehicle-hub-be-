using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMaintenanceServices
{
    public interface IVehicleMaintenanceService
    {
        Task<List<VehicleMaintenance>> GetAllVehicleMaintenances();
        Task<VehicleMaintenance> GetVehicleMaintenance(int id);
        Task<VehicleMaintenance> CreateVehicleMaintenance(VehicleMaintenance vehicleMaintenance);
        Task<VehicleMaintenance> UpdateVehicleMaintenance(int id, VehicleMaintenance vehicleMaintenance);
        Task DeleteVehicleMaintenance(int id);
    }
}
