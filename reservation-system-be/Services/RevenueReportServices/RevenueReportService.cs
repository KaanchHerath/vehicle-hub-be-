using System;
using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.RevenueReportServices
{
    public class RevenueReportService : IRevenueReportService
    {
        private readonly DataContext _context;

        public RevenueReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<RevenueReportDTO>> GetAllRevenue()
        {
            try
            {
                return await _context.Payments
                .Include(p => p.Invoice)
                .Select(p => new RevenueReportDTO
                {
                    id = p.Id,
                    amount = p.Invoice.Amount,
                    type = p.PaymentStatus,
                    date = p.PaymentDate
                })
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting revenue", ex);
            }
                
        }
    }
}