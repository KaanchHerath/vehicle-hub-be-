using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.FeedbackReportService;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    public class FeedbackReportController : Controller
    {

        private readonly IFeedbackReportService _fbc;

        public FeedbackReportController(IFeedbackReportService fbc)
        {
            _fbc = fbc;
        }

        [HttpGet]
        public async Task<ActionResult<List<FeedBackReportDTO>>> GetAll()
        {
            try
            {
                return Ok(await _fbc.GetAllFeedBacks());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
