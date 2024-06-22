using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.CustomerReservationService
{
    public interface ICustomerReservationService
    {
        Task<IEnumerable<CustomerReservationDto>> GetAllCustomerReservations();
        Task<CustomerReservationDto> GetCustomerReservation(int id);
        Task<CustomerReservation> CreateCustomerReservation(CreateCustomerReservationDto customerReservationDto);
        Task<CustomerReservation> UpdateCustomerReservation(int id, CreateCustomerReservationDto customerReservation);
        Task DeleteCustomerReservation(int id);
    }
}
