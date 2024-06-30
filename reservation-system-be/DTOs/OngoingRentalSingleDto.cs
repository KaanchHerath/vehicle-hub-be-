using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct OngoingRentalSingleDto
    (
        int CustomerReservationId,
        int VehicleId,
        string ModelName,
        string Make,
        DateTime StartDate,
        DateTime EndDate,
        Status Status,
        TimeOnly StartTime,
        TimeOnly EndTime,
        string Thumbnail
    );
}
