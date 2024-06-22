using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleModelMakeDto
    (
        int Id,
        string Name,
        int Year,
        int EngineCapacity,
        int SeatingCapacity,
        string Fuel,
        VehicleMake VehicleMake
    );
}
