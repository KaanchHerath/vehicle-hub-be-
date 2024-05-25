﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.InvoiceService
{
    public class InvoiceService : IInvoiceService
    {
        private readonly DataContext _context;

        public InvoiceService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }

        public async Task<Invoice> GetInvoiceById(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task<Invoice> CreateInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> UpdateInvoice(Invoice invoice)
        {
            _context.Entry(invoice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return false;
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

