using reservation_system_be.DTOs;

namespace reservation_system_be.Services.AdminVehicleServices
{
    public interface IAdminVehicleService
    {
        Task CreateVehicleModel(VehicleModelDto createVehicleModelDto);
        Task<IEnumerable<VehicleModelMakeDto>> ViewVehicleModels();
        Task UpdateVehicleModel(int id, VehicleModelDto createVehicleModelDto);
        Task<AdditionalFeaturesDto> ViewVehicleModel(int id);
    }
}
