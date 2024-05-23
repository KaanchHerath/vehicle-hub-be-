using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.RevenueReportService;


namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    public class RevenueReportController : Controller
    {
        private readonly IRevenueReportService _fbc;

        public RevenueReportController(IRevenueReportService fbc)
        {
            _fbc = fbc;
        }

        [HttpGet]
        public async Task<ActionResult<List<RevenueReportDTO>>> GetAll()
        {
            try
            {
                return Ok(await _fbc.GetAllRevenues());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

