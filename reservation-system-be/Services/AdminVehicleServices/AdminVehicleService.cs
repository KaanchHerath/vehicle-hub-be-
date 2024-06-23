﻿using reservation_system_be.Data;
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
        public async Task CreateVehicleModel(VehicleModelDto vehicleModelDto)
        {
            var vehicleModel = await _vehicleModelService.CreateVehicleModel(vehicleModelDto.vehicleModel);
            var additionalFeatures = new AdditionalFeatures
            {
                ABS = vehicleModelDto.additionalFeatures.ABS,
                AcFront = vehicleModelDto.additionalFeatures.AcFront,
                SecuritySystem = vehicleModelDto.additionalFeatures.SecuritySystem,
                Bluetooth = vehicleModelDto.additionalFeatures.Bluetooth,
                ParkingSensor = vehicleModelDto.additionalFeatures.ParkingSensor,
                AirbagDriver = vehicleModelDto.additionalFeatures.AirbagDriver,
                AirbagPassenger = vehicleModelDto.additionalFeatures.AirbagPassenger,
                AirbagSide = vehicleModelDto.additionalFeatures.AirbagSide,
                FogLights = vehicleModelDto.additionalFeatures.FogLights,
                NavigationSystem = vehicleModelDto.additionalFeatures.NavigationSystem,
                Sunroof = vehicleModelDto.additionalFeatures.Sunroof,
                TintedGlass = vehicleModelDto.additionalFeatures.TintedGlass,
                PowerWindow = vehicleModelDto.additionalFeatures.PowerWindow,
                RearWindowWiper = vehicleModelDto.additionalFeatures.RearWindowWiper,
                AlloyWheels = vehicleModelDto.additionalFeatures.AlloyWheels,
                ElectricMirrors = vehicleModelDto.additionalFeatures.ElectricMirrors,
                AutomaticHeadlights = vehicleModelDto.additionalFeatures.AutomaticHeadlights,
                KeylessEntry = vehicleModelDto.additionalFeatures.KeylessEntry,
                VehicleModelId = vehicleModel.Id
            };
            await _additionalFeaturesService.AddAdditionalFeatures(additionalFeatures);
        }

        public async Task<IEnumerable<VehicleModelMakeDto>> ViewVehicleModels()
        {
            var vehicleModels = await _vehicleModelService.GetAllVehicleModels();
            if(vehicleModels == null)
            {
                throw new Exception("Vehicle Models not found");
            }
            return vehicleModels;
        }
        public async Task UpdateVehicleModel(int id, VehicleModelDto vehicleModelDto)
        {
            var existingAdditionalFeatures = await _context.AdditionalFeatures.FirstOrDefaultAsync(a => a.VehicleModelId == id);
            if (existingAdditionalFeatures == null)
            {
                throw new Exception("Additional Features not found");
            }
            await _vehicleModelService.UpdateVehicleModel(id, vehicleModelDto.vehicleModel);
            var additionalFeatures = new AdditionalFeatures
            {
                Id = existingAdditionalFeatures.Id,
                ABS = vehicleModelDto.additionalFeatures.ABS,
                AcFront = vehicleModelDto.additionalFeatures.AcFront,
                SecuritySystem = vehicleModelDto.additionalFeatures.SecuritySystem,
                Bluetooth = vehicleModelDto.additionalFeatures.Bluetooth,
                ParkingSensor = vehicleModelDto.additionalFeatures.ParkingSensor,
                AirbagDriver = vehicleModelDto.additionalFeatures.AirbagDriver,
                AirbagPassenger = vehicleModelDto.additionalFeatures.AirbagPassenger,
                AirbagSide = vehicleModelDto.additionalFeatures.AirbagSide,
                FogLights = vehicleModelDto.additionalFeatures.FogLights,
                NavigationSystem = vehicleModelDto.additionalFeatures.NavigationSystem,
                Sunroof = vehicleModelDto.additionalFeatures.Sunroof,
                TintedGlass = vehicleModelDto.additionalFeatures.TintedGlass,
                PowerWindow = vehicleModelDto.additionalFeatures.PowerWindow,
                RearWindowWiper = vehicleModelDto.additionalFeatures.RearWindowWiper,
                AlloyWheels = vehicleModelDto.additionalFeatures.AlloyWheels,
                ElectricMirrors = vehicleModelDto.additionalFeatures.ElectricMirrors,
                AutomaticHeadlights = vehicleModelDto.additionalFeatures.AutomaticHeadlights,
                KeylessEntry = vehicleModelDto.additionalFeatures.KeylessEntry,
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