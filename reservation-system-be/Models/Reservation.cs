using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Reservation
    {
            public int Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Status Status { get; set; } = Status.Waiting;
            public VehicleLog? VehicleLog { get; set; }
            public Feedback? Feedback { get; set; } 
            public ICollection<Employee>? Employees { get; set; }
            [ForeignKey("CustomerReservationId")]
            public int CustomerReservationId { get; set; }
            [JsonIgnore]
            public CustomerReservation? CusReservation { get; set; }
            public ICollection<Notification> Notification { get; } = new List<Notification>(); 


            public int NoOfDays
            {
                get
                {
                    return (EndDate - StartDate).Days;
                }
            }
    }
    public enum Status
    {
        Waiting,
        Pending,
        Confirmed,
        Cancelled
    }
}


