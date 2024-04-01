using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMakeServices
{
    public interface IVehicleMakeService
    {
        Task<List<VehicleMake>> GetAllVehicleMake();
        Task<VehicleMake?> GetSingleVehicleMake(int id);
        Task<List<VehicleMake>> AddVehicleMake(VehicleMake vehicleMake);
        Task<List<VehicleMake>?> UpdateVehicleMake(int id, VehicleMake request);
        Task<List<VehicleMake>?> DeleteVehicleMake(int id);
    }
}
