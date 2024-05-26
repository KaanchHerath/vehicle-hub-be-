using System.Collections.Generic;
using System.Threading.Tasks;
using reservation_system_be.Models;

namespace reservation_system_be.Services.InvoiceService
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(int id);
        Task<Invoice> CreateInvoice(Invoice invoice);
        Task<Invoice> UpdateInvoice(Invoice invoice);
        Task<bool> DeleteInvoice(int id);
    }
}
