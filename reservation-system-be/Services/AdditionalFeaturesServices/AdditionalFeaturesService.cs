using Microsoft.EntityFrameworkCore;
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

        public async Task<AdditionalFeatures> GetAdditionalFeatures(int id)
        {
            var additionalFeatures = await _context.AdditionalFeatures.FindAsync(id);
            if (additionalFeatures == null)
            {
                throw new Exception("AdditionalFeatures not found");
            }

            return additionalFeatures;
        }

        public async Task<AdditionalFeatures> AddAdditionalFeatures(AdditionalFeatures additionalFeatures)
        {
            _context.AdditionalFeatures.Add(additionalFeatures);
            await _context.SaveChangesAsync();
            return additionalFeatures;
        }

        public async Task<AdditionalFeatures> UpdateAdditionalFeatures(int id, AdditionalFeatures additionalFeatures)
        {
            if (id != additionalFeatures.Id)
            {
                throw new Exception("Id mismatch");
            }

            _context.Entry(additionalFeatures).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return additionalFeatures;
        }

        public async Task DeleteAdditionalFeatures(int id)
        {
            var additionalFeatures = await _context.AdditionalFeatures.FindAsync(id);
            if (additionalFeatures == null)
            {
                throw new Exception("AdditionalFeatures not found");
            }

            _context.AdditionalFeatures.Remove(additionalFeatures);
            await _context.SaveChangesAsync();
        }
    }
}
