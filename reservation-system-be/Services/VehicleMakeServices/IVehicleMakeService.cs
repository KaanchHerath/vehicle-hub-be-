using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMakeServices
{
    public interface IVehicleMakeService
    {
        Task<List<VehicleMake>> GetAllVehicleMakes();
        Task<VehicleMake> GetVehicleMake(int id);
        Task<VehicleMake> CreateVehicleMake([FromForm] VehicleMakeDto vehicleMakeDto, IFormFile file);
        Task<VehicleMake> UpdateVehicleMake(int id, [FromForm] VehicleMakeDto vehicleMakeDto, IFormFile file);
        Task DeleteVehicleMake(int id);
    }
}
