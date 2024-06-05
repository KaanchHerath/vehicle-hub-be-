using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdditionalFeaturesServices
{
    public interface IAdditionalFeaturesService
    {
        Task<IEnumerable<AdditionalFeaturesDto>> GetAllAdditionalFeatures();
        Task<AdditionalFeaturesDto> GetAdditionalFeatures(int id);
        Task<AdditionalFeatures> AddAdditionalFeatures(AdditionalFeatures additionalFeatures);
        Task<AdditionalFeatures> UpdateAdditionalFeatures(int id, AdditionalFeatures additionalFeatures);
        Task DeleteAdditionalFeatures(int id);
    }
}
