using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct CreateReservationDto
    (
        int Id,
        TimeOnly StartTime,
        TimeOnly EndTime,
        DateTime StartDate,
        DateTime EndDate,
        int EmployeeId,
        Status Status
    );
}
