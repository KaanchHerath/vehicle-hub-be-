namespace reservation_system_be.Models
{
    public class Reservation
    {
            public int Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Status { get; set; } = string.Empty;
            public ICollection<Reservation>? Reservations { get; set;}
            public VehicleLog? VehicleLog { get; set; }
            public ICollection<Feedback>? Feedbacks { get; set;}
            public int EmployeeId { get; set; }
            public Employee? Employee { get; set; }
            public CustomerReservation? CusReservation { get; set; }
            public int NoOfDays
            {
                get
                {
                    return (EndDate - StartDate).Days;
                }
            }
    }
}
