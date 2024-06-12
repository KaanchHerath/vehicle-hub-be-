using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Services.VehicleFilterServices;
using System;
using System.Threading.Tasks;

namespace reservation_system_be.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VehicleFilterController : ControllerBase
	{
		private readonly IVehicleFilterService _vehicleFilterService;

		public VehicleFilterController(IVehicleFilterService vehicleFilterService)
		{
			_vehicleFilterService = vehicleFilterService;
		}

		[HttpGet("available")]
		public async Task<IActionResult> GetAvailableVehicles(DateTime startDate, TimeOnly startTime, DateTime endDate, TimeOnly endTime)
		{
			try
			{
				var availableVehicles = await _vehicleFilterService.GetvailableVehiclesDetails(startDate, startTime, endDate, endTime);

				return Ok(availableVehicles);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
