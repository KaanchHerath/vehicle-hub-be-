using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleMaintenanceServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminAndStaffOnly")]
    public class VehicleMaintenanceController : ControllerBase
    {
        private readonly IVehicleMaintenanceService _vehicleMaintenanceService;

        public VehicleMaintenanceController(IVehicleMaintenanceService vehicleMaintenanceService)
        {
            _vehicleMaintenanceService = vehicleMaintenanceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMaintenanceDto>>> GetAllVehicleMaintenances()
        {
            var vehicleMaintenances = await _vehicleMaintenanceService.GetAllVehicleMaintenances();
            return Ok(vehicleMaintenances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMaintenanceDto>> GetVehicleMaintenance(int id)
        {
            try
            {
                var vehicleMaintenance = await _vehicleMaintenanceService.GetVehicleMaintenance(id);
                return Ok(vehicleMaintenance);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleMaintenance>> CreateVehicleMaintenance(CreateVehicleMaintenanceDto createVehicleMaintenanceDto)
        {
            try
            {
                var newVehicleMaintenance = await _vehicleMaintenanceService.CreateVehicleMaintenance(createVehicleMaintenanceDto);
                return Ok(newVehicleMaintenance);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleMaintenance>> UpdateVehicleMaintenance(int id, CreateVehicleMaintenanceDto vehicleMaintenance)
        {
            try
            {
                var updatedVehicleMaintenance = await _vehicleMaintenanceService.UpdateVehicleMaintenance(id, vehicleMaintenance);
                return Ok(updatedVehicleMaintenance);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleMaintenance(int id)
        {
            try
            {
                await _vehicleMaintenanceService.DeleteVehicleMaintenance(id);
                return NoContent();
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
