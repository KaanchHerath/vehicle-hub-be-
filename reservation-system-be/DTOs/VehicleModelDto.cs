using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleModelDto
    (
        int Id,
        string Name,
        VehicleMake VehicleMake,
        string Year,
        string EngineCapacity
    );
}
