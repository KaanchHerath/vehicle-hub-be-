// @bikz {
namespace reservation_system_be.DTOs
{
    public record struct VehicleUtilizationReportDTO
   (
        string vehicleNo,
        DateTime startDate,
        DateTime endDate,
        int mileage,
        int reservationId
   );
}
// } @bikz