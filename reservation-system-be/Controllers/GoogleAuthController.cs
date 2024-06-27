using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reservation_system_be.DTOs;
using reservation_system_be.Services.GoogleService;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly GoogleAuthService _googleAuthService;
        private readonly ILogger<GoogleAuthController> _logger;
        public GoogleAuthController(GoogleAuthService googleAuthService, ILogger<GoogleAuthController> logger)
        {
            _googleAuthService = googleAuthService;
            _logger = logger;
        }

        [HttpGet("google-signin")]
        public IActionResult GoogleSignIn()
        {
            var redirectUrl = Url.Action("GoogleCallBack", "GoogleAuth", null, Request.Scheme);
            var authorizationUrl = _googleAuthService.GetGoogleAuthorizationUrl(redirectUrl);
            return Redirect(authorizationUrl);
        }

        [HttpPost("google-callback")]
        public async Task<IActionResult> GoogleCallback([FromBody] GoogleDTO googleDto)
        {
            try
            {
                if (googleDto == null || string.IsNullOrEmpty(googleDto.Email))
                {
                    _logger.LogWarning("Email parameter is missing or invalid");
                    return BadRequest("Email parameter is missing or invalid");
                }

                var authDto = await _googleAuthService.HandleGoogleCallbackAsync(googleDto.Email);

                if (authDto == null)
                {
                    _logger.LogWarning("Invalid Google Token or user not found");
                    return Unauthorized("Invalid Google Token or user not found");
                }

                return Ok(new { authDto.token, authDto.id, authDto.status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GoogleCallback");
                return StatusCode(500, "Internal Server Error: An error occurred while handling the Google callback");
            }
        }

    }
}
