using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct CustomerReservationDto(
        int CustomerId,
        int VehicleId,
        Reservation Reservation
    );
}
