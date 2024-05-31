using reservation_system_be.DTOs;

namespace reservation_system_be.Services.VehicleUtilizationReportServices
{
    public interface IVehicleUtilizationReportService
    {
        public Task<List<VehicleUtilizationReportDTO>> GetAllVehicleUtilization();
    }
}