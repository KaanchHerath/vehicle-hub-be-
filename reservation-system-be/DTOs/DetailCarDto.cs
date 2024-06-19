namespace reservation_system_be.DTOs
{
    public record struct DetailCarDto
    (
        int Id,
        string Make,
        string Model,
        string Colour,
        int Mileage,
        float CostPerDay,
        float CostPerExtraKM,
        string Transmission,
        int SeatingCapacity,
        int Year,
        int EngineCapacity,
        string FuelType
    );
}