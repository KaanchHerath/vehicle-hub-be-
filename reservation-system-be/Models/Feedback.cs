namespace reservation_system_be.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Feedback_Date { get; set; } 
        public DateTime Feedback_Time { get; set; } 
        public int RatingNo { get; set; }
        public  int ReservationId { get; set; } // Optional foreign key property
        public required Reservation Reservation { get; set; }// Optional reference navigation to principal
    }
}
