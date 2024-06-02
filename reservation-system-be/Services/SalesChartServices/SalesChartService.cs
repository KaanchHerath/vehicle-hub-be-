using reservation_system_be.Data;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;
using reservation_system_be.Services.CusVsFeedServices;

namespace reservation_system_be.Services.CusVsFeedService
{
    public class SalesChartService : ISalesChartService
    {
        private readonly DataContext _context;

        public SalesChartService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SalesChartDTO>> GetAllSales()
        {
            try
            {
                var summaryData = _context.Reservations
                .Where(r => r.Status == Models.Status.Completed)
                .GroupBy(r => r.EndDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalSales = g.Count()
            })
                .ToList()
                .Select(s => new SalesChartDTO
                {
                    name = new DateTime(2024, s.Month, 1).ToString("MMM"),
                    TotalSales = s.TotalSales,
                })
                .ToList();

                return summaryData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting total count of sales", ex);
            }
        }
    }
}

