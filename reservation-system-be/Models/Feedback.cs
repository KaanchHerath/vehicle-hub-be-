using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Designation { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int RatingNo { get; set; }
        public DateTime Feedback_Date { get; set; } 
        public DateTime Feedback_Time { get; set; } 
        
        public  int? ReservationId { get; set; } // Optional foreign key property
        [JsonIgnore]
        public Reservation Reservation { get; set; }// Optional reference navigation to principal
    }
}
