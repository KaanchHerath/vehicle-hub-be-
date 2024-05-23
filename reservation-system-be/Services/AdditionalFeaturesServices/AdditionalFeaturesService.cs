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
            var existingAdditionalFeatures = await _context.AdditionalFeatures.FindAsync(id);
            if (existingAdditionalFeatures == null)
            {
                throw new DataNotFoundException("AdditionalFeatures not found");
            }

            existingAdditionalFeatures.ABS = additionalFeatures.ABS;
            existingAdditionalFeatures.AcFront = additionalFeatures.AcFront;
            existingAdditionalFeatures.SecuritySystem = additionalFeatures.SecuritySystem;
            existingAdditionalFeatures.Bluetooth = additionalFeatures.Bluetooth;
            existingAdditionalFeatures.ParkingSensor = additionalFeatures.ParkingSensor;
            existingAdditionalFeatures.AirbagDriver = additionalFeatures.AirbagDriver;
            existingAdditionalFeatures.AirbagPassenger = additionalFeatures.AirbagPassenger;
            existingAdditionalFeatures.AirbagSide = additionalFeatures.AirbagSide;
            existingAdditionalFeatures.FogLights = additionalFeatures.FogLights;
            existingAdditionalFeatures.NavigationSystem = additionalFeatures.NavigationSystem;
            existingAdditionalFeatures.Sunroof = additionalFeatures.Sunroof;
            existingAdditionalFeatures.TintedGlass = additionalFeatures.TintedGlass;
            existingAdditionalFeatures.PowerWindow = additionalFeatures.PowerWindow;
            existingAdditionalFeatures.RearWindowWiper = additionalFeatures.RearWindowWiper;
            existingAdditionalFeatures.AlloyWheels = additionalFeatures.AlloyWheels;
            existingAdditionalFeatures.ElectricMirrors = additionalFeatures.ElectricMirrors;
            existingAdditionalFeatures.AutomaticHeadlights = additionalFeatures.AutomaticHeadlights;
            existingAdditionalFeatures.KeylessEntry = additionalFeatures.KeylessEntry;

            _context.Entry(existingAdditionalFeatures).State = EntityState.Modified;
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
