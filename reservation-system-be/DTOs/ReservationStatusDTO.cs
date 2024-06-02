// 
namespace reservation_system_be.DTOs
{
    public record struct ReservationStatusDTO
   (
        int ConfirmedCount,
        int PendingCount,
        int CancelledCount 
   );
}
// 
