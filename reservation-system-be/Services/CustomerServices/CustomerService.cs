using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Helper;
using reservation_system_be.Models;

namespace reservation_system_be.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    throw new DataNotFoundException("Customer not found");
                }
                return customer;
            }
        }


        public async Task<Customer> AddCustomer(Customer customer)
        {
           await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Customer> GetCustomerByOtp(string otp)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.PasswordResetOtp == otp);
        }

        public async Task<Customer> UpdateCustomer(int id, Customer customer)
        {
            

            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                throw new DataNotFoundException("Customer not found");
            }

            // Update customer properties
            existingCustomer.Name = customer.Name;
            existingCustomer.NIC = customer.NIC;
            existingCustomer.DrivingLicenseNo = customer.DrivingLicenseNo;
            existingCustomer.Email = customer.Email;
            existingCustomer.Status = customer.Status;
            existingCustomer.Address = customer.Address;
            existingCustomer.ContactNo = customer.ContactNo;

            if (!string.IsNullOrEmpty(customer.Password))
            {
                // Hash the new password
                existingCustomer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
            }

            _context.Entry(existingCustomer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingCustomer;

        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new DataNotFoundException("Customer not found");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

        }


    }
}
