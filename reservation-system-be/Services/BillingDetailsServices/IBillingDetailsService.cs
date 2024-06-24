using System.Collections.Generic;
using System.Threading.Tasks;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.BillingDetailsServices
{
    public interface IBillingDetailsService
    {
        Task<IEnumerable<BillingDetailsDTO>> GetAllBillingDetailsAsync();
        Task<BillingDetailsDTO?> GetBillingDetailByIdAsync(int id);
    }
}
