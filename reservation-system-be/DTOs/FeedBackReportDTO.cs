using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct FeedBackReportDTO
   (
        int id,
        string vehicle,
        string content,
        int rating,
        DateTime date,
        string customer
   );
}