namespace reservation_system_be.DTOs
{
    public record struct DashboardStatusDTO
   (
        int salesTot,
        int salesLastMonth,
        int feedbackTot,
        int feedbackLastMonth,
        int customerTot,
        int customerLastMonth,
        int reservationsTot,
        int reservationsLastMonth
   );
}
