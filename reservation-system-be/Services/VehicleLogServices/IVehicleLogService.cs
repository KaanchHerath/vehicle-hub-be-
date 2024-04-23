using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleLogServices
{
    public interface IVehicleLogService
    {
        Task<List<VehicleLog>> GetAllVehicleLogs();
        Task<VehicleLog> GetVehicleLog(int id);
        Task<VehicleLog> CreateVehicleLog(VehicleLog vehicleLog);
        Task<VehicleLog> UpdateVehicleLog(int id, VehicleLog vehicleLog);
        Task DeleteVehicleLog(int id);
    }
}
