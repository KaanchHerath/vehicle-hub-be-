using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
     public record struct FeedBackReportDTO
    (
     int Id,
     string Type, 
     string Content,
     int RatingNo,
     DateTime Feedback_Date,
     string Customername
    );
}
