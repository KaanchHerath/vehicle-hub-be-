using reservation_system_be.Models;
namespace reservation_system_be.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllVehicles();

        Task<Vehicle> GetVehicle(int id);

        Task<Vehicle> CreateVehicle(Vehicle vehicle);

        Task<Vehicle> UpdateVehicle(int id, Vehicle vehicle);

        Task DeleteVehicle(int id);

        Task<List<VehicleResponse>> SearchVehicle(String search);

        Task<List<VehicleResponse>> GetAllVehiclesDetails();

        Task<List<VehicleResponse>> FilterVehicles(int? vehicleTypeId, int? vehicleMakeId, int? seatingCapacity, float? depositAmount);
    }
}
