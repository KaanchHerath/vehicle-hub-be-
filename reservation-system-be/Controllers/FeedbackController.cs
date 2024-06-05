using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.FeedbackServices;
using reservation_system_be.Services.VehicleMakeServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackservice)
        {
            _feedbackService = feedbackservice;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _feedbackService.GetAllFeedbacks();

                if (feedbacks.Any())
                {
                    return Ok(feedbacks);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Feedback>>> AddFeedbacks(FeedbackRequest feedback)
        {
            try
            {
                var newfeedback = await _feedbackService.AddFeedbacks(feedback);
                return Ok(newfeedback);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{type}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks(String type)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacks(type);

                if (feedbacks.Any())
                {
                    return Ok(feedbacks);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("vehicle/{vehicleid}")]
        public async Task<ActionResult<IEnumerable<FeedbackResponse>>> GetFeedbacksForVehicle(int vehicleid)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksForVehicle(vehicleid);

                if (feedbacks.Any())
                {
                    return Ok(feedbacks);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
