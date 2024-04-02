namespace reservation_system_be.Models
{
    public class VehicleAvailability
    {
        public int Id { get; set; }
        public int ReservationId {  get; set; }
        public Reservation Reservation { get; set; } = null!;
        public bool Availability {  get; set; }
    }
}
