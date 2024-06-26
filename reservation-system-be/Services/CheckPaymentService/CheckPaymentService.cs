using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.CheckPaymentService
{
    public class CheckPaymentService : ICheckPaymentService
    {
        private readonly DataContext _context;

        public CheckPaymentService(DataContext context)
        {
            _context = context;
        }

        public async Task<PaymentCheckResponseDto> CheckPaymentForInvoice(int invoiceId)
        {
            var payment = await _context.Payments
                .Where(p => p.InvoiceId == invoiceId)
                .Select(p => new PaymentCheckResponseDto
                {
                    PaymentExists = true,
                    PaymentStatus = p.PaymentStatus,
                    PaymentMethod = p.PaymentMethod
                })
                .FirstOrDefaultAsync();

            return payment ?? new PaymentCheckResponseDto { PaymentExists = false };
        }
    }
}
