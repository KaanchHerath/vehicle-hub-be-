using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        public float ExtraCostPerKM { get; set; }
        public string Transmission { get; set; } = string.Empty;
        [ForeignKey("VehicleTypeId")]
        public int VehicleTypeId { get; set; }
        [JsonIgnore]
        public VehicleType? VehicleType { get; set; }
        [JsonIgnore]
        public VehicleInsurance? VehicleInsurance { get; set; }
        [JsonIgnore]
        public ICollection<VehicleMaintenance>? VehicleMaintenance { get; set; }

        [ForeignKey("VehicleModelId")]
        public int VehicleModelId { get; set; }
        [JsonIgnore]
        public VehicleModel? VehicleModel { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
        [JsonIgnore]
        public ICollection <Wishlist>? Wishlist { get; set; }
        [JsonIgnore]
        public ICollection<CustomerReservation>? CusReservation { get; set; }
        [JsonIgnore]
        public ICollection<VehiclePhoto>? VehiclePhoto { get; set; }
    }
}