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
            public bool IsRead { get; set; } = false;
        }
    

}

