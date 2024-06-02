using System;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.RevenueReportServices
{
    public interface IRevenueReportService
    {
        public Task<List<RevenueReportDTO>> GetAllRevenue();
    }
}