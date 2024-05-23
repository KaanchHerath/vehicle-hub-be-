using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleModelServices;

namespace reservation_system_be.Services.AdditionalFeaturesServices
{
    public class AdditionalFeaturesService : IAdditionalFeaturesService
    {
        private readonly DataContext _context;
        private readonly IVehicleModelService _vehicleModelService;

        public AdditionalFeaturesService(DataContext context, IVehicleModelService vehicleModelService)
        {
            _context = context;
            _vehicleModelService = vehicleModelService;
        }

        public async Task<IEnumerable<AdditionalFeaturesDto>> GetAllAdditionalFeatures()
        {
            var additionalFeatures = await _context.AdditionalFeatures
                .Include(a => a.VehicleModel)
                .ToListAsync();

            if (additionalFeatures == null || !additionalFeatures.Any())
            {
                throw new Exception("No additional features found");
            }

            var additionalFeaturesDtos = new List<AdditionalFeaturesDto>();

            foreach (var additionalFeature in additionalFeatures)
            {
                var additionalFeaturesDto = new AdditionalFeaturesDto
                {
                    Id = additionalFeature.Id,
                    ABS = additionalFeature.ABS,
                    AcFront = additionalFeature.AcFront,
                    SecuritySystem = additionalFeature.SecuritySystem,
                    Bluetooth = additionalFeature.Bluetooth,
                    ParkingSensor = additionalFeature.ParkingSensor,
                    AirbagDriver = additionalFeature.AirbagDriver,
                    AirbagPassenger = additionalFeature.AirbagPassenger,
                    AirbagSide = additionalFeature.AirbagSide,
                    FogLights = additionalFeature.FogLights,
                    NavigationSystem = additionalFeature.NavigationSystem,
                    Sunroof = additionalFeature.Sunroof,
                    TintedGlass = additionalFeature.TintedGlass,
                    PowerWindow = additionalFeature.PowerWindow,
                    RearWindowWiper = additionalFeature.RearWindowWiper,
                    AlloyWheels = additionalFeature.AlloyWheels,
                    ElectricMirrors = additionalFeature.ElectricMirrors,
                    AutomaticHeadlights = additionalFeature.AutomaticHeadlights,
                    KeylessEntry = additionalFeature.KeylessEntry,
                    VehicleModel = await _vehicleModelService.GetVehicleModel(additionalFeature.VehicleModelId)
                };
                additionalFeaturesDtos.Add(additionalFeaturesDto);
            }
            return additionalFeaturesDtos;
        }

        public async Task<AdditionalFeaturesDto> GetAdditionalFeatures(int id)
        {
            var additionalFeatures = await _context.AdditionalFeatures
                .Include(a => a.VehicleModel)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (additionalFeatures == null)
            {
                throw new Exception("AdditionalFeatures not found");
            }

            var additionalFeaturesDto = new AdditionalFeaturesDto
            {
                Id = additionalFeatures.Id,
                ABS = additionalFeatures.ABS,
                AcFront = additionalFeatures.AcFront,
                SecuritySystem = additionalFeatures.SecuritySystem,
                Bluetooth = additionalFeatures.Bluetooth,
                ParkingSensor = additionalFeatures.ParkingSensor,
                AirbagDriver = additionalFeatures.AirbagDriver,
                AirbagPassenger = additionalFeatures.AirbagPassenger,
                AirbagSide = additionalFeatures.AirbagSide,
                FogLights = additionalFeatures.FogLights,
                NavigationSystem = additionalFeatures.NavigationSystem,
                Sunroof = additionalFeatures.Sunroof,
                TintedGlass = additionalFeatures.TintedGlass,
                PowerWindow = additionalFeatures.PowerWindow,
                RearWindowWiper = additionalFeatures.RearWindowWiper,
                AlloyWheels = additionalFeatures.AlloyWheels,
                ElectricMirrors = additionalFeatures.ElectricMirrors,
                AutomaticHeadlights = additionalFeatures.AutomaticHeadlights,
                KeylessEntry = additionalFeatures.KeylessEntry,
                VehicleModel = await _vehicleModelService.GetVehicleModel(additionalFeatures.VehicleModelId)
            };

            return additionalFeaturesDto;
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
            existingAdditionalFeatures.VehicleModelId = additionalFeatures.VehicleModelId;

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
