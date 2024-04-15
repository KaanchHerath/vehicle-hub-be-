using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdditionalFeaturesServices
{
    public class AdditionalFeaturesService : IAdditionalFeaturesService
    {
        private readonly DataContext _context;

        public AdditionalFeaturesService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<AdditionalFeatures>> GetAllAdditionalFeatures()
        {
            return await _context.AdditionalFeatures.ToListAsync();
        }

        public async Task<AdditionalFeatures> GetSingleAdditionalFeatures(int id)
        {
            return await _context.AdditionalFeatures.FindAsync(id);
        }

        public Task<List<AdditionalFeatures>> AddAdditionalFeatures(AdditionalFeatures additionalFeatures)
        {
            throw new NotImplementedException();
        }
        public Task<List<AdditionalFeatures>> UpdateAdditionalFeatures(int id, AdditionalFeatures request)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdditionalFeatures>> DeleteteAdditionalFeatures(int id)
        {
            throw new NotImplementedException();
        }
    }
}
