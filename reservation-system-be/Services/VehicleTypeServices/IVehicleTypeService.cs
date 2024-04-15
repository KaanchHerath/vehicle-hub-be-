using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleTypeServices
{
    public interface IVehicleTypeService
    {
        Task<List<VehicleType>> GetAllVehicleType();
        Task<VehicleType?> GetSingleVehicleType(int id);
        Task<List<VehicleType>> AddVehicleType(VehicleType vehicleType);
        Task<List<VehicleType>?> UpdateVehicleType(int id, VehicleType request);
        Task<List<VehicleType>?> DeleteVehicleType(int id);
    }
}
