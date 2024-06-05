using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleInsuranceServices
{
    public interface IVehicleInsuranceService
    {
        Task<IEnumerable<VehicleInsuranceDto>> GetAllVehicleInsurances();
        Task<VehicleInsuranceDto> GetSingleVehicleInsurance(int id);
        Task<VehicleInsurance> CreateVehicleInsurance(VehicleInsurance vehicleInsurance);
        Task<VehicleInsurance> UpdateVehicleInsurance(int id, VehicleInsurance vehicleInsurance);
        Task DeleteVehicleInsurance(int id);
    }
}
