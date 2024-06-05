using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleModelServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IVehicleModelService _vehicleModelService;

        public VehicleModelController(IVehicleModelService vehicleModelService)
        {
            _vehicleModelService = vehicleModelService;
        }

        [HttpGet]
        public async Task<ActionResult<List<IEnumerable<VehicleModelDto>>>> GetAllVehicleModels()
        {
            var vehicleModels = await _vehicleModelService.GetAllVehicleModels();
            return Ok(vehicleModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleModelDto>> GetVehicleModel(int id)
        {
            try
            {
                var vehicleModel = await _vehicleModelService.GetVehicleModel(id);
                return Ok(vehicleModel);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<VehicleModel>> CreateVehicleModel(VehicleModel vehicleModel)
        {
            var newVehicleModel = await _vehicleModelService.CreateVehicleModel(vehicleModel);
            return Ok(newVehicleModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleModel>> UpdateVehicleModel(int id, VehicleModel vehicleModel)
        {
            try
            {
                var updatedVehicleModel = await _vehicleModelService.UpdateVehicleModel(id, vehicleModel);
                return Ok(updatedVehicleModel);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicleModel(int id)
        {
            try
            {
                await _vehicleModelService.DeleteVehicleModel(id);
                return Ok();
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
