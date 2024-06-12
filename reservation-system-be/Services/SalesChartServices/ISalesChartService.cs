using reservation_system_be.DTOs;

namespace reservation_system_be.Services.CusVsFeedServices
{
    public interface ISalesChartService
    {
        public Task<List<SalesChartDTO>> GetAllSales();
    }
}
