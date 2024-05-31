using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.CustomerVehicleServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookNowController : ControllerBase
    {
        private readonly IBookNowService _bookNowService;

        public BookNowController(IBookNowService bookNowService)
        {
            _bookNowService = bookNowService;
        }

        [HttpGet]
        public async Task<IEnumerable<BookNowDto>> GetAllCards()
        {
            return await _bookNowService.GetAllCards();
        }
    }
}
