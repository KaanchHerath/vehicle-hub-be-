namespace reservation_system_be.DTOs
{
    public record struct BookNowDto
    (
        int VehicleId,
        string Name,
        string Make,
        string Type,
        int Year,
        string Transmission,
        int SeatingCapacity,
        float CostPerDay
    );
}
