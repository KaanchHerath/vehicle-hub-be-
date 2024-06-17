using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.CustomerVehicleServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontVehicleServiceController : ControllerBase
    {
        private readonly IFrontVehicleService _bookNowService;

        public FrontVehicleServiceController(IFrontVehicleService bookNowService)
        {
            _bookNowService = bookNowService;
        }

        [HttpGet("Details")]
        public async Task<ActionResult<IEnumerable<BookNowDto>>> GetAllCards()
        {
            return Ok(await _bookNowService.GetAllCards());
        }

        [HttpGet("Images/{id}")]
        public async Task<ActionResult<VehicleImagesDto>> GetImages(int id)
        {
            return Ok(await _bookNowService.GetImages(id));
        }

        [HttpGet("AdditionalFeatures/{id}")]
        public async Task<ActionResult<CreateAdditionalFeaturesDto>> GetAdditionalFeatures(int id)
        {
            return Ok(await _bookNowService.GetAdditionalFeatures(id));
        }
    }
}
