using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleMakeServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleMakeController : ControllerBase
    {
        private readonly IVehicleMakeService _vehicleMakeService;

        public VehicleMakeController(IVehicleMakeService vehicleMakeService)
        {
            _vehicleMakeService = vehicleMakeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMake>>> GetAllVehicleMakes()
        {
            var vehicleMakes = await _vehicleMakeService.GetAllVehicleMakes();
            return Ok(vehicleMakes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMake>> GetVehicleMake(int id)
        {
            try
            {
                var vehicleMake = await _vehicleMakeService.GetVehicleMake(id);
                return Ok(vehicleMake);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleMake>> AddVehicleMake(VehicleMake vehicleMake)
        {
            var newVehicleMake = await _vehicleMakeService.CreateVehicleMake(vehicleMake);
            return CreatedAtAction(nameof(GetVehicleMake), new { id = newVehicleMake.Id }, newVehicleMake);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleMake>> UpdateVehicleMake(int id, VehicleMake vehicleMake)
        {
            try
            {
                var updatedVehicleMake = await _vehicleMakeService.UpdateVehicleMake(id, vehicleMake);
                return Ok(updatedVehicleMake);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMake(int id)
        {
            try
            {
                await _vehicleMakeService.DeleteVehicleMake(id);
                return NoContent();
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
