namespace reservation_system_be.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Payment? Payment { get; set; }
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

    }
}

