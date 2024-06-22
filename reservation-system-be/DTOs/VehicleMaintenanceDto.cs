using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleMaintenanceDto
    (
        int Id,
        DateTime Date,
        String Description,
        String Type,
        int CurrentMileage,
        int VehicleId,
        string RegistrationNumber
    );
}
