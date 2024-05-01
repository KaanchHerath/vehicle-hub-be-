using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;
using reservation_system_be.Services.AdditionalFeaturesServices;

namespace reservation_system_be.Services.VehicleModelServices
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly DataContext _context;
        private readonly IAdditionalFeaturesService _additionalFeaturesService;

        public VehicleModelService(DataContext context, IAdditionalFeaturesService additionalFeaturesService)
        {
            _context = context;
            _additionalFeaturesService = additionalFeaturesService;
        }

        public async Task<List<VehicleModel>> GetAllVehicleModels()
        {
            return await _context.VehicleModels.ToListAsync();
        }

        public async Task<VehicleModel> GetVehicleModel(int id)
        {
            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }
            return vehicleModel;
        }

        public async Task<VehicleModel> CreateVehicleModel(CreateVehicleModelDto createVehicleModelDto)
        {
            var vehicleModel = new VehicleModel
            {
                Name = createVehicleModelDto.VehicleModel.Name,
                Year = createVehicleModelDto.VehicleModel.Year,
                EngineCapacity = createVehicleModelDto.VehicleModel.EngineCapacity,
                SeatingCapacity = createVehicleModelDto.VehicleModel.SeatingCapacity,
                Fuel = createVehicleModelDto.VehicleModel.Fuel,
                VehicleMakeId = createVehicleModelDto.VehicleModel.VehicleMakeId
            };
            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();

            var additionalFeatures = new AdditionalFeatures
            {
                ABS = createVehicleModelDto.AdditionalFeatures.ABS,
                AcFront = createVehicleModelDto.AdditionalFeatures.AcFront,
                SecuritySystem = createVehicleModelDto.AdditionalFeatures.SecuritySystem,
                Bluetooth = createVehicleModelDto.AdditionalFeatures.Bluetooth,
                ParkingSensor = createVehicleModelDto.AdditionalFeatures.ParkingSensor,
                AirbagDriver = createVehicleModelDto.AdditionalFeatures.AirbagDriver,
                AirbagPassenger = createVehicleModelDto.AdditionalFeatures.AirbagPassenger,
                AirbagSide = createVehicleModelDto.AdditionalFeatures.AirbagSide,
                FogLights = createVehicleModelDto.AdditionalFeatures.FogLights,
                NavigationSystem = createVehicleModelDto.AdditionalFeatures.NavigationSystem,
                Sunroof = createVehicleModelDto.AdditionalFeatures.Sunroof,
                TintedGlass = createVehicleModelDto.AdditionalFeatures.TintedGlass,
                PowerWindow = createVehicleModelDto.AdditionalFeatures.PowerWindow,
                RearWindowWiper = createVehicleModelDto.AdditionalFeatures.RearWindowWiper,
                AlloyWheels = createVehicleModelDto.AdditionalFeatures.AlloyWheels,
                ElectricMirrors = createVehicleModelDto.AdditionalFeatures.ElectricMirrors,
                AutomaticHeadlights = createVehicleModelDto.AdditionalFeatures.AutomaticHeadlights,
                KeylessEntry = createVehicleModelDto.AdditionalFeatures.KeylessEntry,
                VehicleModelId = vehicleModel.Id
            };
            await _additionalFeaturesService.AddAdditionalFeatures(additionalFeatures);

            return vehicleModel;
        }

        public async Task<VehicleModel> UpdateVehicleModel(int id, VehicleModel vehicleModel)
        {
            var existingVehicleModel = await _context.VehicleModels.FindAsync(id);
            if (existingVehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }
            existingVehicleModel.Name = vehicleModel.Name;
            existingVehicleModel.VehicleMakeId = vehicleModel.VehicleMakeId;
            _context.Entry(existingVehicleModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleModel;
        }

        public Task DeleteVehicleModel(int id)
        {
            var vehicleModel = _context.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }
            _context.VehicleModels.Remove(vehicleModel);
            return _context.SaveChangesAsync();
        }

       

       
    }
}
