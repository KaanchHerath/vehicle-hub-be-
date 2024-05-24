using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdditionalFeaturesServices
{
    public interface IAdditionalFeaturesService
    {
        Task<IEnumerable<AdditionalFeaturesDto>> GetAllAdditionalFeatures();
        Task<AdditionalFeaturesDto> GetAdditionalFeatures(int id);
        Task AddAdditionalFeatures(CreateVehicleModelDto createVehicleModelDto);
        Task UpdateAdditionalFeatures(int id, CreateVehicleModelDto createVehicleModelDto);
        Task DeleteAdditionalFeatures(int id);
    }
}
