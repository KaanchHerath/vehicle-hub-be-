namespace reservation_system_be.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } 
        public DateTime Feedback_Date { get; set; } 
        public DateTime Feedback_Time { get; set; } 
        public int RatingNo { get; set; }
        public  int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
