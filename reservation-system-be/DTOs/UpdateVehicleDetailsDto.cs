using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct UpdateVehicleDetailsDto
    (
        int Id,
        string RegistrationNumber,
        string ChassisNo,
        string Colour,
        int Mileage,
        float CostPerDay,
        float CostPerExtraKM,
        string Transmission,
        bool Status,
        int VehicleTypeId,
        int VehicleModelId,
        int EmployeeId
    );
}
