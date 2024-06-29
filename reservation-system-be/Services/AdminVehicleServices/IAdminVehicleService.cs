using reservation_system_be.DTOs;

namespace reservation_system_be.Services.AdminVehicleServices
{
    public interface IAdminVehicleService
    {
        Task CreateVehicleModel(VehicleModelDto createVehicleModelDto);
        Task<IEnumerable<VehicleModelMakeDto>> ViewVehicleModels();
        Task UpdateVehicleModel(int id, VehicleModelDto createVehicleModelDto);
        Task<AdditionalFeaturesDto> ViewVehicleModel(int id);
        Task<VehicleHoverDto> GetVehicleHover(string regNo);
        Task<VehicleModelHoverDto> GetVehicleModelHover(int id);
        Task<VehicleMaintenanceDescriptionHoverDto> GetVehicleMaintenanceDescription(int id);
    }
}
