using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleTypeServices
{
    public interface IVehicleTypeService
    {
        Task<List<VehicleType>> GetAllVehicleTypes();
        Task<VehicleType?> GetSingleVehicleType(int id);
        Task<VehicleType> CreateVehicleType(VehicleType vehicleType);
        Task<VehicleType> UpdateVehicleType(int id, VehicleType vehicleType);
        Task DeleteVehicleType(int id);
    }
}
