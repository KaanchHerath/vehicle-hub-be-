using reservation_system_be.Models;

namespace reservation_system_be.Services.AdditionalFeaturesServices
{
    public interface IAdditionalFeaturesService
    {
        Task<List<AdditionalFeatures>> GetAllAdditionalFeatures();
        Task<AdditionalFeatures> GetAdditionalFeatures(int id);
        Task<AdditionalFeatures> AddAdditionalFeatures(AdditionalFeatures additionalFeatures);
        Task<AdditionalFeatures> UpdateAdditionalFeatures(int id, AdditionalFeatures additionalFeatures);
        Task DeleteAdditionalFeatures(int id);
    }
}
