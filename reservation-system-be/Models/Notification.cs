namespace reservation_system_be.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Generated_DateTime { get; set; }
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

    }
}
