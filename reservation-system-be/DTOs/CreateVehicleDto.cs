using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public class CreateVehicleDto
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string ChassisNo { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public float CostPerDay { get; set; }
        public float CostPerExtraKM { get; set; }
        public string Transmission { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int VehicleTypeId { get; set; }
        public int VehicleModelId { get; set; }
        public int EmployeeId { get; set; }
    }
}
