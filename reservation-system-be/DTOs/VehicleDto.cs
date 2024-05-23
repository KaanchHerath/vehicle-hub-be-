using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleDto
    (
        int Id,
        string RegistrationNumber,
        string ChassisNo,
        string Colour,
        int Mileage,
        float CostPerDay,
        string Transmission,
        VehicleType VehicleType,
        VehicleModelDto VehicleModel,
        Employee Employee
    );
}
