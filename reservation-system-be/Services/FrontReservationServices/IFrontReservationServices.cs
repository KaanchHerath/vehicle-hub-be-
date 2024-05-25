using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.FrontReservationServices
{
    public interface IFrontReservationServices
    {
        Task<CustomerReservation> RequestReservations(CreateCustomerReservationDto customerReservationDto);
    }
}
