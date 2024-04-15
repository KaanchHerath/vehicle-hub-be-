using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleInsuranceServices
{
    public interface IVehicleInsuranceService
    {
        Task<List<VehicleInsurance>> GetAllVehicleInsurance();
        Task<VehicleInsurance?> GetSingleVehicleInsurance(int id);
        Task<List<VehicleInsurance>> AddVehicleInsurance(VehicleInsurance vehicleInsurance);
        Task<List<VehicleInsurance>?> UpdateVehicleInsurance(int id, VehicleInsurance request);
        Task<List<VehicleInsurance>?> DeleteVehicleInsurance(int id);
    }
}
