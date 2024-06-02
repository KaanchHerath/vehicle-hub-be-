using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMakeServices
{
    public interface IVehicleMakeService
    {
        Task<List<VehicleMake>> GetAllVehicleMakes();
        Task<VehicleMake> GetVehicleMake(int id);
        Task<VehicleMake> CreateVehicleMake(string name,IFormFile logo);
        Task<VehicleMake> UpdateVehicleMake(int id, VehicleMake vehicleMake);
        Task DeleteVehicleMake(int id);
    }
}
