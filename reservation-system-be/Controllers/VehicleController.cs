using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VehicleDto>>> GetAllVehicles()
        {
            {
                var vehicles = await _vehicleService.GetAllVehicles();
                return Ok(vehicles);
            }
        }
        

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle(int id)
        {
            try
            {
                return await _vehicleService.GetVehicle(id);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle([FromForm]CreateVehicleDto createVehicleDto, IFormFile formFile, IFormFile front, IFormFile rear, IFormFile dashboard, IFormFile interior)
        {
            try
            {
                var vehicle = await _vehicleService.CreateVehicle(createVehicleDto, formFile, front, rear, dashboard, interior);
                return Ok(vehicle);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("Details/{id}")]
        public async Task<ActionResult<Vehicle>> UpdateVehicle(int id,UpdateVehicleDetailsDto createVehicleDto)
        {
            try
            {
                var vehicle = await _vehicleService.UpdateVehicle(id, createVehicleDto);
                return Ok(vehicle);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("Thumbnail/{id}")]
        public async Task<ActionResult> UpdateThumbnail(int id, IFormFile formFile)
        { 
            try
            {
                await _vehicleService.UpdateThumbnail(id, formFile);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("FrontImg/{id}")]
        public async Task<ActionResult> UpdateFrontImg(int id,IFormFile front)
        {
            try
            {
                await _vehicleService.UpdateFrontImg(id, front);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("RearImg/{id}")]
        public async Task<ActionResult> UpdateRearImg(int id, IFormFile rear)
        {
            try
            {
                await _vehicleService.UpdateRearImg(id, rear);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("DashboardImg/{id}")]
        public async Task<ActionResult> UpdateDashboardImg(int id, IFormFile dashboard)
        {
            try
            {
                await _vehicleService.UpdateDashboardImg(id, dashboard);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("InteriorImg/{id}")]
        public async Task<ActionResult> UpdateInteriorImg(int id, IFormFile interior)
        {
            try
            {
                await _vehicleService.UpdateInteriorImg(id, interior);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            try
            {
                await _vehicleService.DeleteVehicle(id);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("alldata")]
        public async Task<ActionResult<List<VehicleResponse>>> GetAllVehiclesDetails()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesDetails();
                return Ok(vehicles);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        

    }
}
