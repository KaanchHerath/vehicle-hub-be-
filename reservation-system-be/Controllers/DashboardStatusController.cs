using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.DashboardStatusServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardStatusController : Controller
    {
        private readonly IDashboardStatusService _dsc;

        public DashboardStatusController(IDashboardStatusService dsc)
        {
            _dsc = dsc;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardStatusDTO>> GetAll()
        {
            try
            {
                return Ok(await _dsc.GetAllDashboardStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}