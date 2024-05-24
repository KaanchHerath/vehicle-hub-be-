using reservation_system_be.Models;

namespace reservation_system_be.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomer(int id);
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> UpdateCustomer(int id, Customer customer);
        Task DeleteCustomer(int id);
        Task<Customer> GetCustomerByEmail(string email);
    }
}
