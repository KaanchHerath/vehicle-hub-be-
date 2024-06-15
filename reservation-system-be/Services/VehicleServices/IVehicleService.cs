using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
namespace reservation_system_be.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllVehicles();
        Task<VehicleDto> GetVehicle(int id);

        Task<Vehicle> CreateVehicle([FromForm]CreateVehicleDto createVehicleDto, IFormFile formFile, IFormFile front, IFormFile rear, IFormFile dashboard, IFormFile interior);
        Task<Vehicle> UpdateVehicle(int id, UpdateVehicleDetailsDto createVehicleDto);
        Task UpdateThumbnail(int id,IFormFile formFile);
        Task UpdateFrontImg(int id, IFormFile front);
        Task UpdateRearImg(int id,IFormFile rear);
        Task UpdateDashboardImg(int id,IFormFile dashboard);
        Task UpdateInteriorImg(int id, IFormFile interior);
        Task DeleteVehicle(int id);

        Task<List<VehicleResponse>> GetAllVehiclesDetails();

    }
}
