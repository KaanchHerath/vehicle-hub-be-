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

        public async Task AddAdditionalFeatures(CreateVehicleModelDto createVehicleModelDto)
        {
            var vehicleModel = await _vehicleModelService.CreateVehicleModel(createVehicleModelDto.vehicleModel);
            var additionalFeatures = new AdditionalFeatures
            {
                ABS = createVehicleModelDto.additionalFeatures.ABS,
                AcFront = createVehicleModelDto.additionalFeatures.AcFront,
                SecuritySystem = createVehicleModelDto.additionalFeatures.SecuritySystem,
                Bluetooth = createVehicleModelDto.additionalFeatures.Bluetooth,
                ParkingSensor = createVehicleModelDto.additionalFeatures.ParkingSensor,
                AirbagDriver = createVehicleModelDto.additionalFeatures.AirbagDriver,
                AirbagPassenger = createVehicleModelDto.additionalFeatures.AirbagPassenger,
                AirbagSide = createVehicleModelDto.additionalFeatures.AirbagSide,
                FogLights = createVehicleModelDto.additionalFeatures.FogLights,
                NavigationSystem = createVehicleModelDto.additionalFeatures.NavigationSystem,
                Sunroof = createVehicleModelDto.additionalFeatures.Sunroof,
                TintedGlass = createVehicleModelDto.additionalFeatures.TintedGlass,
                PowerWindow = createVehicleModelDto.additionalFeatures.PowerWindow,
                RearWindowWiper = createVehicleModelDto.additionalFeatures.RearWindowWiper,
                AlloyWheels = createVehicleModelDto.additionalFeatures.AlloyWheels,
                ElectricMirrors = createVehicleModelDto.additionalFeatures.ElectricMirrors,
                AutomaticHeadlights = createVehicleModelDto.additionalFeatures.AutomaticHeadlights,
                KeylessEntry = createVehicleModelDto.additionalFeatures.KeylessEntry,
                VehicleModelId = vehicleModel.Id
            };
            _context.AdditionalFeatures.Add(additionalFeatures);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdditionalFeatures(int id, CreateVehicleModelDto createvehicleModelDto)
        {
            var existingVehicleModel = await _vehicleModelService.GetVehicleModel(id);
            
            existingVehicleModel.Name = createvehicleModelDto.vehicleModel.Name;
            existingVehicleModel.VehicleMake.Id = createvehicleModelDto.vehicleModel.VehicleMakeId;
            existingVehicleModel.Year = createvehicleModelDto.vehicleModel.Year;
            existingVehicleModel.EngineCapacity = createvehicleModelDto.vehicleModel.EngineCapacity;
            existingVehicleModel.SeatingCapacity = createvehicleModelDto.vehicleModel.SeatingCapacity;
            existingVehicleModel.Fuel = createvehicleModelDto.vehicleModel.Fuel;

            var vehicleModel = new VehicleModel
            {
                Id = existingVehicleModel.Id,
                Name = existingVehicleModel.Name,
                VehicleMakeId = existingVehicleModel.VehicleMake.Id,
                Year = existingVehicleModel.Year,
                EngineCapacity = existingVehicleModel.EngineCapacity,
                SeatingCapacity = existingVehicleModel.SeatingCapacity,
                Fuel = existingVehicleModel.Fuel
            };

            await _vehicleModelService.UpdateVehicleModel(id, vehicleModel); 

            var existingAdditionalFeatures = await _context.AdditionalFeatures.FirstOrDefaultAsync(a => a.VehicleModelId == existingVehicleModel.Id);
            if(existingAdditionalFeatures == null)
            {
                throw new DataNotFoundException("AdditionalFeatures not found");
            }
            
            existingAdditionalFeatures.ABS = createvehicleModelDto.additionalFeatures.ABS;
            existingAdditionalFeatures.AcFront = createvehicleModelDto.additionalFeatures.AcFront;
            existingAdditionalFeatures.SecuritySystem = createvehicleModelDto.additionalFeatures.SecuritySystem;
            existingAdditionalFeatures.Bluetooth = createvehicleModelDto.additionalFeatures.Bluetooth;
            existingAdditionalFeatures.ParkingSensor = createvehicleModelDto.additionalFeatures.ParkingSensor;
            existingAdditionalFeatures.AirbagDriver = createvehicleModelDto.additionalFeatures.AirbagDriver;
            existingAdditionalFeatures.AirbagPassenger = createvehicleModelDto.additionalFeatures.AirbagPassenger;
            existingAdditionalFeatures.AirbagSide = createvehicleModelDto.additionalFeatures.AirbagSide;
            existingAdditionalFeatures.FogLights = createvehicleModelDto.additionalFeatures.FogLights;
            existingAdditionalFeatures.NavigationSystem = createvehicleModelDto.additionalFeatures.NavigationSystem;
            existingAdditionalFeatures.Sunroof = createvehicleModelDto.additionalFeatures.Sunroof;
            existingAdditionalFeatures.TintedGlass = createvehicleModelDto.additionalFeatures.TintedGlass;
            existingAdditionalFeatures.PowerWindow = createvehicleModelDto.additionalFeatures.PowerWindow;
            existingAdditionalFeatures.RearWindowWiper = createvehicleModelDto.additionalFeatures.RearWindowWiper;
            existingAdditionalFeatures.AlloyWheels = createvehicleModelDto.additionalFeatures.AlloyWheels;
            existingAdditionalFeatures.ElectricMirrors = createvehicleModelDto.additionalFeatures.ElectricMirrors;
            existingAdditionalFeatures.AutomaticHeadlights = createvehicleModelDto.additionalFeatures.AutomaticHeadlights;
            existingAdditionalFeatures.KeylessEntry = createvehicleModelDto.additionalFeatures.KeylessEntry;
            existingAdditionalFeatures.VehicleModelId = existingVehicleModel.Id;

            _context.Entry(existingAdditionalFeatures).State = EntityState.Modified;

            await _context.SaveChangesAsync();
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
