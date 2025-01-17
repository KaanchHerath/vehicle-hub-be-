﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class CustomerReservation
    {
        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [ForeignKey("ReservationId")]
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        [JsonIgnore]
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        [JsonIgnore]
        public VehicleLog? VehicleLog { get; set; }
        [JsonIgnore]
        public Feedback? Feedback { get; set; }
        [JsonIgnore]
        public ICollection<Notification> Notifications { get; } = new List<Notification>();
    }
}
