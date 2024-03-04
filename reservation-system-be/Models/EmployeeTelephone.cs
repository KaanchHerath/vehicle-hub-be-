namespace reservation_system_be.Models
{
    public class EmployeeTelephone
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
