using reservation_system_be.DTOs;
using reservation_system_be.Models;
namespace reservation_system_be.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetAllVehicles();
        Task<VehicleDto> GetVehicle(int id);
        Task<Vehicle> CreateVehicle(Vehicle vehicle);
        Task<Vehicle> UpdateVehicle(int id, Vehicle vehicle);
        Task DeleteVehicle(int id);
        Task<List<Vehicle>> SearchVehicle(String search);
    }
}
