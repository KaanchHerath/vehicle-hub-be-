using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Reservation
    {
            public int Id { get; set; }
            [JsonConverter(typeof(TimeOnlyJsonConverter))]
            public DateTime StartTime { get; set; }
            [JsonConverter(typeof(TimeOnlyJsonConverter))]
            public DateTime EndTime { get; set; }
            [JsonConverter(typeof(DateOnlyJsonConverter))]
            public DateTime StartDate { get; set; }
            [JsonConverter(typeof(DateOnlyJsonConverter))]
            public DateTime EndDate { get; set; }
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Status Status { get; set; } = Status.Waiting;
            [JsonIgnore]
            public VehicleLog? VehicleLog { get; set; }
            [JsonIgnore]
            public Feedback? Feedback { get; set; }
            [JsonIgnore]
            public CustomerReservation? CustomerReservation { get; set; }
            [JsonIgnore]
            public ICollection<Notification> Notifications { get; } = new List<Notification>();
            [ForeignKey("EmployeeId")]
            public int? EmployeeId { get; set; }
            [JsonIgnore]
            public Employee? Employee { get; set; }
            [JsonIgnore]
            public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
            [JsonIgnore]
            public VehicleAvailability? VehicleAvailability { get; set; }
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


