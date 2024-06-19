using System.ComponentModel.DataAnnotations;

namespace reservation_system_be.DTOs
{
    public class EmployeePasswordDTO
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
