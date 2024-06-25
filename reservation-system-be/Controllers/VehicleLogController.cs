using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleLogServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleLogController : ControllerBase
    {
        private readonly IVehicleLogService _vehicleLogService;

        public VehicleLogController(IVehicleLogService vehicleLogService)
        {
            _vehicleLogService = vehicleLogService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminAndStaffOnly")]
        public async Task<ActionResult<IEnumerable<VehicleLog>>> GetAllVehicleLogs()
        {
            var vehicleLogs = await _vehicleLogService.GetAllVehicleLogs();
            return Ok(vehicleLogs);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminAndStaffOnly")]
        public async Task<ActionResult<VehicleLog>> GetVehicleLog(int id)
        {
            try
            {
                var vehicleLog = await _vehicleLogService.GetVehicleLog(id);
                return Ok(vehicleLog);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminAndStaffOnly")]
        public async Task<ActionResult<VehicleLog>> AddVehicleLog(VehicleLog vehicleLog)
        {
            var newVehicleLog = await _vehicleLogService.CreateVehicleLog(vehicleLog);
            return CreatedAtAction(nameof(GetVehicleLog), new { id = newVehicleLog.Id }, newVehicleLog);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminAndStaffOnly")]
        public async Task<ActionResult<VehicleLog>> UpdateVehicleLog(int id, VehicleLogDto vehicleLog)
        {
            try
            {
                var updatedVehicleLog = await _vehicleLogService.UpdateVehicleLog(id, vehicleLog);
                return Ok(updatedVehicleLog);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteVehicleLog(int id)
        {
            try
            {
                await _vehicleLogService.DeleteVehicleLog(id);
                return NoContent();
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
