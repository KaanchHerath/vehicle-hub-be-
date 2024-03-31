using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
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
        public async Task<ActionResult<List<VehicleMake>>> GetAllVehicleMake()
        {
            return await _vehicleMakeService.GetAllVehicleMake();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMake>> GetSingleVehicleMake(int id)
        {
            var vehicleMake = await _vehicleMakeService.GetSingleVehicleMake(id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            return vehicleMake;
        }

        [HttpPost]
        public async Task<ActionResult<List<VehicleMake>>> AddVehicleMake(VehicleMakeCreateDTO vehicleMake)
        {
            var newVehicleMake = new VehicleMake
            {
                Name = vehicleMake.Name,
                Logo = vehicleMake.Logo
            };

            return await _vehicleMakeService.AddVehicleMake(newVehicleMake);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<VehicleMake>>> UpdateVehicleMake(int id, VehicleMakeCreateDTO vehicleMake)
        {
            var updatedVehicleMake = new VehicleMake
            {
                Name = vehicleMake.Name,
                Logo = vehicleMake.Logo
            };

            var result = await _vehicleMakeService.UpdateVehicleMake(id, updatedVehicleMake);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<VehicleMake>>> DeleteVehicleMake(int id)
        {
            var result = await _vehicleMakeService.DeleteVehicleMake(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }   
    }
}
