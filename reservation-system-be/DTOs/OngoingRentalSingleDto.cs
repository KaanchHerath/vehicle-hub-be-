using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct OngoingRentalSingleDto
    (
        int CustomerReservationId,
        string Name,
        string ModelName,
        DateTime StartDate,
        DateTime EndDate,
        Status Status,
        TimeOnly StartTime,
        TimeOnly EndTime
    );
}
