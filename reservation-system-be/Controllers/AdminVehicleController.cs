using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Services.AdminVehicleServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminVehicleController : ControllerBase
    {
        public readonly IAdminVehicleService _adminVehicleService;

        public AdminVehicleController(IAdminVehicleService adminVehicleService)
        {
            _adminVehicleService = adminVehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateVehicleModelDto>>> ViewVehicleModels()
        {
            try
            {
                var vehicleModels = await _adminVehicleService.ViewVehicleModels();
                return Ok(vehicleModels);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdditionalFeaturesDto>> ViewVehicleModel(int id)
        {
            try
            {
                var additionalFeaturesDto = await _adminVehicleService.ViewVehicleModel(id);
                return Ok(additionalFeaturesDto);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicleModel(VehicleModelDto createVehicleModelDto)
        {
            try
            {
                await _adminVehicleService.CreateVehicleModel(createVehicleModelDto);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVehicleModel(int id, VehicleModelDto createVehicleModelDto)
        {
            try
            {
                await _adminVehicleService.UpdateVehicleModel(id, createVehicleModelDto);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        [HttpGet("regNo")]
        public async Task<ActionResult<VehicleHoverDto>> GetVehicleHover(string regNo)
        {
            try
            {
                var vehicleHoverDto = await _adminVehicleService.GetVehicleHover(regNo);
                return Ok(vehicleHoverDto);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpGet("hover/{id}")]
        public async Task<ActionResult<VehicleModelHoverDto>> GetVehicleModelHover(int id)
        {
            try
            {
                var vehicleModelHoverDto = await _adminVehicleService.GetVehicleModelHover(id);
                return Ok(vehicleModelHoverDto);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
