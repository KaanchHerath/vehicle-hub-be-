using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
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
        public async Task<ActionResult<List<Vehicle>>> GetAllVehicles()
        {
            return await _vehicleService.GetAllVehicles();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
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
        public async Task<ActionResult<Vehicle>> CreateVehicle(Vehicle vehicle)
        {
            return await _vehicleService.CreateVehicle(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Vehicle>> UpdateVehicle(int id, Vehicle vehicle)
        {
            try
            {
                return await _vehicleService.UpdateVehicle(id, vehicle);
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

        [HttpGet("search")]
        public async Task<ActionResult<List<Vehicle>>> SearchVehicle(String search)
        {
            try
            {
                return await _vehicleService.SearchVehicle(search);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
