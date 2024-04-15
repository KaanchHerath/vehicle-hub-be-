using reservation_system_be.Models;

namespace reservation_system_be.Services.AdditionalFeaturesServices
{
    public interface IAdditionalFeaturesService
    {
        Task<List<AdditionalFeatures>> GetAllAdditionalFeatures();
        Task<AdditionalFeatures> GetSingleAdditionalFeatures(int id);
        Task<List<AdditionalFeatures>> AddAdditionalFeatures(AdditionalFeatures additionalFeatures);
        Task<List<AdditionalFeatures>> UpdateAdditionalFeatures(int id,AdditionalFeatures request);
        Task<List<AdditionalFeatures>> DeleteteAdditionalFeatures(int id);
    }
}
