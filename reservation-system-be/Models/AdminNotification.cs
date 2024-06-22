using System.ComponentModel.DataAnnotations.Schema;

namespace reservation_system_be.Models
{
    public class AdminNotification
   
        {
            public int Id { get; set; }
            public string Type { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime Generated_DateTime { get; set; }
           
            [ForeignKey("VehicleInsuranceID")]
            public int? VehicleInsuranceID { get; set; } = null;
           
            public VehicleInsurance? VehicleInsurance { get; set; }

            [ForeignKey("VehicleMaintenanceId")]
            public int? VehicleMaintenanceId { get; set; } = null;
        
            public VehicleMaintenance? VehicleMaintenance { get; set; }

        }
    

}
}
