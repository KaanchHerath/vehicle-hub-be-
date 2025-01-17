﻿using reservation_system_be.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Feedback
    {
        public int Id { get; set; }
      
        public string Type { get; set; } = string.Empty;
        public string Vehicle_Review { get; set; } = string.Empty;
        public string Service_Review { get; set; } = string.Empty;
        public int RatingNo { get; set; }
        public DateTime Feedback_Date { get; set; } 
        public DateTime Feedback_Time { get; set; }
        [ForeignKey("CustomerReservationId")]
        public  int CustomerReservationId { get; set; } // Optional foreign key property
        [JsonIgnore]
        public CustomerReservation? CustomerReservation { get; set; }// Optional reference navigation to principal
    }
}
