using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleUtilizationReportServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleUtilizationReportController : Controller 
    {
        private readonly IVehicleUtilizationReportService _vuc;

        public VehicleUtilizationReportController(IVehicleUtilizationReportService vuc)
        {
            _vuc = vuc; 
        }

        [HttpGet]
        public async Task<ActionResult<List<VehicleUtilizationReportDTO>>> GetAll()
        {
            try
            {
                return Ok(await _vuc.GetAllVehicleUtilization());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}