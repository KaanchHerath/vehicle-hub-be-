﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Reservation
    {
            public int Id { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss}")]
            public DateTime StartTime { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm:ss}")]
            public DateTime EndTime { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime StartDate { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime EndDate { get; set; }
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public Status Status { get; set; } = Status.Waiting;
            [JsonIgnore]
            public string Status { get; set; } = string.Empty;
            [JsonIgnore]
            public VehicleLog? VehicleLog { get; set; }
            [JsonIgnore]
            public Feedback? Feedback { get; set; }
            [JsonIgnore]
            public ICollection<Employee>? Employees { get; set; }

            [JsonIgnore]
            [JsonIgnore]
            public Feedback? Feedback { get; set; }
            [JsonIgnore]
            public CustomerReservation? CusReservation { get; set; }
            [JsonIgnore]
            public ICollection<Notification> Notification { get; } = new List<Notification>(); 
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


