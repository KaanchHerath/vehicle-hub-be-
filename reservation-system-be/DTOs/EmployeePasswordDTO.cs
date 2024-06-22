using System.ComponentModel.DataAnnotations;

namespace reservation_system_be.DTOs
{
    public class EmployeePasswordDTO
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
