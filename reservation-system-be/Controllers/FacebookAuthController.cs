using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using reservation_system_be.DTOs;
using reservation_system_be.Services.FacebookService;
using System.Net.Http;
using System.Threading.Tasks;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookAuthController : ControllerBase
    {
        private readonly FacebookAuthService _facebookAuthService;
        private readonly ILogger<FacebookAuthController> _logger;
        private readonly IConfiguration _config;

        public FacebookAuthController(FacebookAuthService facebookAuthService, ILogger<FacebookAuthController> logger, IConfiguration config)
        {
            _facebookAuthService = facebookAuthService;
            _logger = logger;
            _config = config;
        }

        [HttpGet("facebook-signin")]
        public IActionResult FacebookSignIn(string redirectUrl)
        {
            var authorizationUrl = _facebookAuthService.GetFacebookAuthorizationUrl(redirectUrl);
            return Redirect(authorizationUrl);
        }

        [HttpPost("facebook-callback")]
        public async Task<IActionResult> FacebookCallback([FromBody] AccessTokenDTO accessTokenDto)
        {
            if (accessTokenDto == null || string.IsNullOrEmpty(accessTokenDto.AccessToken))
            {
                return BadRequest("Invalid access token in the request");
            }

            try
            {
                var authDto = await _facebookAuthService.HandleFacebookCallAsync(accessTokenDto.AccessToken);
                return Ok(authDto); // Return the authentication DTO (token, id, status)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Facebook callback");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to process Facebook callback");
            }
        }

    }
}
