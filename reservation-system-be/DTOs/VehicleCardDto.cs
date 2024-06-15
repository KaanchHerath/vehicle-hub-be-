namespace reservation_system_be.DTOs
{
    public record struct VehicleCardDto
    (
        int Id,
        string Name,
        string Make,
        string Type,
        int Year,
        string Transmission,
        int SeatingCapacity,
        float CostPerDay,
        string Thumbnail
    );
}
