using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct ViewReservationDto
    (
        int Id,
        string Name,
        string Email,
        int Phone,
        string RegNo,
        DateTime StartDate,
        DateTime EndDate,
        TimeOnly StartTime,
        TimeOnly EndTime,
        Status Status
    );
}
