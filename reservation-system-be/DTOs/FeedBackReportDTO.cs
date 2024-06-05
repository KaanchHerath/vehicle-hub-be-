using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct FeedBackReportDTO
   (
        int id,
        string vehicle,
        string vehicle_Review,
        string service_Review,
        int rating,
        DateTime date,
        string customer
   );
}