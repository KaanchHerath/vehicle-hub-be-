using System;
using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.DashboardStatusServices
{
    public class DashboardStatusService : IDashboardStatusService
    {
        private readonly DataContext _context;

        public DashboardStatusService(DataContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatusDTO> GetAllDashboardStatus()
        {
            try
            {
                var today = DateTime.UtcNow;
                var lastMonth = today.AddDays(-30);     // last month mean 30 days before from today

                // Total counts
                var salesTot = _context.Reservations.Where(r => r.Status == Status.Completed).Count();
                var feedbackTot = _context.Feedbacks.Count();
                var customerTot = _context.Customers.Count();
                var reservationsTot = _context.Reservations.Count();

                // Last 30 days counts
                var salesLastMonth = _context.Reservations
                    .Where(r => r.Status == Status.Completed && r.EndDate >= lastMonth).Count();

                var feedbackLastMonth = _context.Feedbacks
                    .Where(f => f.Feedback_Date >= lastMonth).Count();

                var customerLastMonth = 0;

                var reservationsLastMonth = _context.Reservations
                    .Where(r => r.StartDate >= lastMonth).Count();

                return new DashboardStatusDTO(
                    salesTot,
                    salesLastMonth,
                    feedbackTot,
                    feedbackLastMonth,
                    customerTot,
                    customerLastMonth,
                    reservationsTot,
                    reservationsLastMonth
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting vehicle dashboard status", ex);
            }
        }
    }
}
