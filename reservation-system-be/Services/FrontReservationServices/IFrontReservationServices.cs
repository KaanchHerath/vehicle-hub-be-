using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.FrontReservationServices
{
    public interface IFrontReservationServices
    {
        Task<CustomerReservation> RequestReservations(CreateCustomerReservationDto customerReservationDto);
        Task<IEnumerable<OngoingRentalDto>> OngoingRentals(int id);
        Task<OngoingRentalSingleDto> OngoingRentalSingle(int id);
    }
}
