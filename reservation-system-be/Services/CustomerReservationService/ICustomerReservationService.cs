using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.CustomerReservationService
{
    public interface ICustomerReservationService
    {
        Task<List<CustomerReservation>> GetAllCustomerReservations();
        Task<CustomerReservation> GetCustomerReservation(int id);
        Task<CustomerReservation> CreateCustomerReservation(CustomerReservationDto customerReservationDto);
        Task<CustomerReservation> UpdateCustomerReservation(int id, CustomerReservationDto customerReservationDto);
        Task DeleteCustomerReservation(int id);
    }
}
