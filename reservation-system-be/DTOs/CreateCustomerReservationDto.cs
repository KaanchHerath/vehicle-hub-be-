using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct CreateCustomerReservationDto(
        int CustomerId,
        int VehicleId,
        Reservation Reservation
    );
}
