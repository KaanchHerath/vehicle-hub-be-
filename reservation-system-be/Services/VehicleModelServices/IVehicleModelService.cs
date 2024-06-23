using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleModelServices
{
    public interface IVehicleModelService
    {
        Task<VehicleModel> CreateVehicleModel(CreateVehicleModelDto VehicleModel);
        Task<VehicleModel> UpdateVehicleModel(int id, CreateVehicleModelDto vehicleModel);
        Task<VehicleModelMakeDto> GetVehicleModel(int id);
        Task<IEnumerable<VehicleModelMakeDto>> GetAllVehicleModels();
        Task DeleteVehicleModel(int id);
    }
}
