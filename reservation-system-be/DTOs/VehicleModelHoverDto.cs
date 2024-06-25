namespace reservation_system_be.DTOs
{
    public record struct VehicleModelHoverDto
    (
        int Id,
        int VehicleModelId,
        string Name,
        int Year,
        int EngineCapacity,
        int SeatingCapacity,
        string Fuel,
        string Make
    );
}
