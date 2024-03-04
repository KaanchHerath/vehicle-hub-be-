namespace reservation_system_be.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string NIC { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<EmployeeTelephone>? employeeTelephones { get; set; }
    }
}
