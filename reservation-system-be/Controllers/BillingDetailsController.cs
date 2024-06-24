using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Services.BillingDetailsServices;
using System.Threading.Tasks;

namespace reservation_system_be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillingDetailsController : Controller
    {
        private readonly IBillingDetailsService _billingDetailsService;

        public BillingDetailsController(IBillingDetailsService billingDetailsService)
        {
            _billingDetailsService = billingDetailsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var billingDetails = await _billingDetailsService.GetAllBillingDetailsAsync();
            return Ok(billingDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var billingDetail = await _billingDetailsService.GetBillingDetailByIdAsync(id);
            if (billingDetail == null)
            {
                return NotFound();
            }
            return Ok(billingDetail);
        }
    }
}
