using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;

namespace reservation_system_be.Services.BillingDetailsServices
{
    public class BillingDetailsService : IBillingDetailsService
    {
        private readonly DataContext _context;

        public BillingDetailsService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BillingDetailsDTO>> GetAllBillingDetailsAsync()
        {
            return await _context.Invoices
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Customer)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Reservation)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Vehicle)
                .Select(invoice => new BillingDetailsDTO(
                    invoice.Id,
                    invoice.Amount,
                    invoice.DateCreated,
                    invoice.CustomerReservation.Reservation.Status,
                    invoice.CustomerReservation.CustomerId,
                    invoice.CustomerReservation.Customer.Name,
                    invoice.CustomerReservation.Vehicle.RegistrationNumber
                ))
                .ToListAsync();
        }

        public async Task<BillingDetailsDTO?> GetBillingDetailByIdAsync(int id)
        {
            return await _context.Invoices
                .Where(invoice => invoice.Id == id)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Customer)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Reservation)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Vehicle)
                .Select(invoice => new BillingDetailsDTO(
                    invoice.Id,
                    invoice.Amount,
                    invoice.DateCreated,
                    invoice.CustomerReservation.Reservation.Status,
                    invoice.CustomerReservation.CustomerId,
                    invoice.CustomerReservation.Customer.Name,
                    invoice.CustomerReservation.Vehicle.RegistrationNumber
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BillingDetailsDTO>> GetBillingDetailsByCustomerIdAsync(int customerId)
        {
            return await _context.Invoices
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Customer)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Reservation)
                .Include(invoice => invoice.CustomerReservation)
                    .ThenInclude(cr => cr.Vehicle)
                .Where(invoice => invoice.CustomerReservation.CustomerId == customerId)
                .Select(invoice => new BillingDetailsDTO(
                    invoice.Id,
                    invoice.Amount,
                    invoice.DateCreated,
                    invoice.CustomerReservation.Reservation.Status,
                    invoice.CustomerReservation.CustomerId,
                    invoice.CustomerReservation.Customer.Name,
                    invoice.CustomerReservation.Vehicle.RegistrationNumber
                ))
                .ToListAsync();
        }
    }
}
