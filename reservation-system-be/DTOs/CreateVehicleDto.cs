namespace reservation_system_be.DTOs
{
    public class CreateVehicleDto
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string ChassisNo { get; set; }
        public string Colour { get; set; }
        public int Mileage { get; set; }
        public int CostPerDay { get; set; }
        public int CostPerExtraKM { get; set; }
        public string Transmission { get; set; }
        public bool Status { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleModelId { get; set; }
        public int EmployeeId { get; set; }
    }
}
