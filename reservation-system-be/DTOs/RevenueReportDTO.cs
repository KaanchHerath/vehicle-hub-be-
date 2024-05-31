using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct RevenueReportDTO
   (
        int id,
        float amount,
        string type,
        DateTime date
   );
}