using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CusVsFeedServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesChartController : Controller
    {
        private readonly ISalesChartService _scc;

        public SalesChartController(ISalesChartService scc)
        {
            _scc = scc;
        }

        [HttpGet]
        public async Task<ActionResult<List<SalesChartDTO>>> GetAll()
        {
            try
            {
                return Ok(await _scc.GetAllSales());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}