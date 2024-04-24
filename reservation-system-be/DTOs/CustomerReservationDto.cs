using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct CustomerReservationDto
    (
        int Id,
        Customer Customer,
        Vehicle Vehicle,
        Reservation Reservation
    );
}
