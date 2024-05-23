using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.CustomerSideServices
{
    public interface ICustomerSideServices
    {
        Task<CustomerReservation> RequestReservations(CreateCustomerReservationDto customerReservationDto);
    }
}
