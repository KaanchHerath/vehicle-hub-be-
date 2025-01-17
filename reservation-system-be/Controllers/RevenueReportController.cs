﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.RevenueReportServices;

//malith

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Policy = "AdminOnly")]
    [ApiController]
    public class RevenueReportController : ControllerBase
    {
        private readonly IRevenueReportService _rc;

        public RevenueReportController(IRevenueReportService rc)
        {
            _rc = rc;
        }

        [HttpGet]
        public async Task<ActionResult<List<RevenueReportDTO>>> GetAll()
        {
            try
            {
                return Ok(await _rc.GetAllRevenue());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
