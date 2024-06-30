using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct CreateVehicleModelDto
    (
        int Id,
        string Name,
        int VehicleMakeId,
        int Year,
        int EngineCapacity,
        int SeatingCapacity,
        string Fuel
    );
}
