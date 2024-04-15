using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using reservation_system_be.DTOs;
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
        public async Task<ActionResult<List<VehicleType>>> GetAllVehicleType()
        {
            return await _vehicleTypeService.GetAllVehicleType();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetSingleVehicleType(int id)
        {
            var vehicleType = await _vehicleTypeService.GetSingleVehicleType(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return vehicleType;
        }

        [HttpPost]
        public async Task<ActionResult<List<VehicleType>>> AddVehicleType(VehicleTypeCreateDTO vehicleType)
        {
            var newVehicleType = new VehicleType
            {
                Name = vehicleType.Name,
                DepositAmount = vehicleType.DepositAmount
            };
            return await _vehicleTypeService.AddVehicleType(newVehicleType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<VehicleType>>> UpdateVehicleType(int id,VehicleTypeCreateDTO vehicleType)
        {
            var updatedVehicleType = new VehicleType
            {
                Name = vehicleType.Name,
                DepositAmount = vehicleType.DepositAmount
            };
            var result = await _vehicleTypeService.UpdateVehicleType(id, updatedVehicleType);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpDelete]
        public async Task<ActionResult<List<VehicleType>>> DeleteVehicleType(int id)
        {
            var result = await _vehicleTypeService.DeleteVehicleType(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}
