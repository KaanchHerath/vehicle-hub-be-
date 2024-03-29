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
            public VehicleLog? VehicleLog { get; set; }
            public ICollection<Feedback>? Feedbacks { get; set;}
            public ICollection<Employee>? Employees { get; set; }
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
