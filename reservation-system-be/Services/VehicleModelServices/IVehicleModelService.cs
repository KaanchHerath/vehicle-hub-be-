﻿using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleModelServices
{
    public interface IVehicleModelService
    {
        Task<VehicleModel> CreateVehicleModel(CreateVehicleModelDto createVehicleModelDto);
        Task<VehicleModel> UpdateVehicleModel(int id, VehicleModel vehicleModel);
        Task<VehicleModel> GetVehicleModel(int id);
        Task<List<VehicleModel>> GetAllVehicleModels();
        Task DeleteVehicleModel(int id);
    }
}
