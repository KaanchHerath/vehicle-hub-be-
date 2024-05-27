using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.AdditionalFeaturesServices;
using reservation_system_be.Services.VehicleModelServices;
using Microsoft.EntityFrameworkCore;
using System;

namespace reservation_system_be.Services.AdminVehicleServices
{
    public class AdminVehicleService : IAdminVehicleService
    {
        private readonly DataContext _context;
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IAdditionalFeaturesService _additionalFeaturesService;
       
        public AdminVehicleService(DataContext context,IVehicleModelService vehicleModelService,IAdditionalFeaturesService additionalFeaturesService) 
        {
            _context = context;
            _vehicleModelService = vehicleModelService;
            _additionalFeaturesService = additionalFeaturesService;
        }
        public async Task CreateVehicleModel(CreateVehicleModelDto createVehicleModelDto)
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
            await _additionalFeaturesService.AddAdditionalFeatures(additionalFeatures);
        }

        public async Task<IEnumerable<AdditionalFeaturesDto>> ViewVehicleModels()
        {
            return await _additionalFeaturesService.GetAllAdditionalFeatures();
        }

        public async Task UpdateVehicleModel(int id, CreateVehicleModelDto createVehicleModelDto)
        {
            var existingAdditionalFeatures = await _context.AdditionalFeatures.FirstOrDefaultAsync(a => a.VehicleModelId == id);
            if (existingAdditionalFeatures == null)
            {
                throw new Exception("Additional Features not found");
            }
            await _vehicleModelService.UpdateVehicleModel(id, createVehicleModelDto.vehicleModel);
            var additionalFeatures = new AdditionalFeatures
            {
                Id = existingAdditionalFeatures.Id,
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
                VehicleModelId = id
            };
            await _additionalFeaturesService.UpdateAdditionalFeatures(existingAdditionalFeatures.Id, additionalFeatures);
        }

        public async Task<AdditionalFeaturesDto> ViewVehicleModel(int id)
        {
            var additionalFeatures = await _context.AdditionalFeatures.FirstOrDefaultAsync(a => a.VehicleModelId == id);
            if (additionalFeatures == null)
            {
                throw new Exception("Additional Features not found");
            }
            return await _additionalFeaturesService.GetAdditionalFeatures(additionalFeatures.Id);

        }
    }
}