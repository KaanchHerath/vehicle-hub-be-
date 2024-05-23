using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleModelServices
{
    public interface IVehicleModelService
    {
        Task<VehicleModel> CreateVehicleModel(VehicleModel vehicleModel);
        Task<VehicleModel> UpdateVehicleModel(int id, VehicleModel vehicleModel);
        Task<VehicleModelDto> GetVehicleModel(int id);
        Task<IEnumerable<VehicleModelDto>> GetAllVehicleModels();
        Task DeleteVehicleModel(int id);
    }
}
