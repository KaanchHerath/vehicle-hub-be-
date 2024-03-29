using System.ComponentModel.DataAnnotations;

namespace reservation_system_be.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string ChassisNo { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public float CostPerDay { get; set; }
        public string Transmission { get; set; } = string.Empty;
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; } = null!;
        public VehicleInsurance? VehicleInsurance { get; set; }
        public int VehicleMaintenanceId { get; set; }
        public Vehicle VehicleMaintenance { get; set; } = null!;
        public int VehicleModelId { get; set; }
        public VehicleModel? VehicleModel { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public List<WishlistVehicle>? WishlistVehicles { get; set; }
        public CustomerReservation? CusReservation { get; set; }
        public ICollection<VehiclePhoto>? VehiclePhoto { get; set; }
    }
}