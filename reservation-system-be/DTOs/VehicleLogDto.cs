namespace reservation_system_be.DTOs
{
    public record struct VehicleLogDto
    (
        int Id,
        int EndMileage,
        int Penalty,
        string Description,
        int CustomerReservationId
    );
}
