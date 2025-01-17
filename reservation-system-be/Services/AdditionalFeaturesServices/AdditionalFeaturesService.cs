﻿using Microsoft.EntityFrameworkCore;
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
                var vehicleModel = await _context.VehicleModels.FirstOrDefaultAsync(v => v.Id == additionalFeature.VehicleModelId);
                if (vehicleModel == null)
                {
                    throw new DataNotFoundException("VehicleModel not found");
                }
                var additionalFeaturesDto = new AdditionalFeaturesDto
                {
                    additionalFeatures = additionalFeature,
                    vehicleModel = vehicleModel
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
            var vehicleModel = await _context.VehicleModels.FirstOrDefaultAsync(v => v.Id == additionalFeatures.VehicleModelId);
            if (vehicleModel == null)
            {
                throw new DataNotFoundException("VehicleModel not found");
            }
            
            var additionalFeaturesDto = new AdditionalFeaturesDto
            {
               additionalFeatures = additionalFeatures,
               vehicleModel = vehicleModel
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
            if(existingAdditionalFeatures == null)
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
            return existingAdditionalFeatures;
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
