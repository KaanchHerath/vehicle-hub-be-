namespace reservation_system_be.DTOs
{
    public record struct CreateVehicleMaintenanceDto
    (
        int Id,
        DateTime Date,
        String Description,
        String Type,
        int CurrentMileage,
        int VehicleId
    );
}
