using reservation_system_be.DTOs;

namespace reservation_system_be.Services.AdminVehicleServices
{
    public interface IAdminVehicleService
    {
        Task CreateVehicleModel(CreateVehicleModelDto createVehicleModelDto);
        Task<IEnumerable<AdditionalFeaturesDto>> ViewVehicleModels();
        Task UpdateVehicleModel(int id, CreateVehicleModelDto createVehicleModelDto);
        Task<AdditionalFeaturesDto> ViewVehicleModel(int id);
    }
}
