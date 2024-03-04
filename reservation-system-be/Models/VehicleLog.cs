namespace reservation_system_be.Models
{
    public class VehicleLog
    {
        public int Id { get; set; }
        public int EndMileage { get; set; }
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
