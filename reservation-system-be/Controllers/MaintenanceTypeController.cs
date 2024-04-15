using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.MaintenanceTypeServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTypeController : ControllerBase
    {
        private readonly IMaintenanceTypeService _maintenanceTypeService;

        public MaintenanceTypeController(IMaintenanceTypeService maintenanceTypeService)
        {
            _maintenanceTypeService = maintenanceTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaintenanceType>>> GetAllMaintenanceType()
        {
            return await _maintenanceTypeService.GetAllMaintenanceType();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceType>> GetSingleMaintenanceType(int id)
        {
            var maintenanceType = await _maintenanceTypeService.GetSingleMaintenanceType(id);
            if (maintenanceType == null)
            {
                return NotFound();
            }

            return maintenanceType;
        }

        [HttpPost]
        public async Task<ActionResult<List<MaintenanceType>>> AddMaintenanceType(MaintenanceTypeCreateDTO maintenanceType)
        {
            var newMaintenanceType = new MaintenanceType
            {
                Name = maintenanceType.Name,
                MaintenanceId = maintenanceType.MaintenanceId
            };

            return await _maintenanceTypeService.AddMaintenanceType(newMaintenanceType);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<MaintenanceType>>> UpdateMaintenanceType(int id, MaintenanceTypeCreateDTO maintenanceType)
        {
            var updatedMaintenanceType = new MaintenanceType
            {
                Name = maintenanceType.Name,
                MaintenanceId = maintenanceType.MaintenanceId
            };

            var result = await _maintenanceTypeService.UpdateMaintenanceType(id, updatedMaintenanceType);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<MaintenanceType>>> DeleteMaintenanceType(int id)
        {
            var result = await _maintenanceTypeService.DeleteMaintenanceType(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
