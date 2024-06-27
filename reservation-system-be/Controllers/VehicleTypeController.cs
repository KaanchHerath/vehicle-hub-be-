using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleTypeServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;

        public VehicleTypeController(IVehicleTypeService vehicleTypeService)
        {
            _vehicleTypeService = vehicleTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetAllVehicleTypes()
        {
            var vehicleTypes = await _vehicleTypeService.GetAllVehicleTypes();
            return Ok(vehicleTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetSingleVehicleType(int id)
        {
            try
            {
                var vehicleType = await _vehicleTypeService.GetSingleVehicleType(id);
                return Ok(vehicleType);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleType>> AddVehicleType(VehicleType vehicleType)
        {
            var newVehicleType = await _vehicleTypeService.CreateVehicleType(vehicleType);
            return CreatedAtAction(nameof(GetSingleVehicleType), new { id = newVehicleType.Id }, newVehicleType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleType>> UpdateVehicleType(int id,VehicleType vehicleType)
        {
            try
            {
                var updatedVehicleType = await _vehicleTypeService.UpdateVehicleType(id, vehicleType);
                return Ok(updatedVehicleType);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            try
            {
                await _vehicleTypeService.DeleteVehicleType(id);
                return NoContent();
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
