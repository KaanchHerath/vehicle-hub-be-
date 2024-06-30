using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static reservation_system_be.Controllers.RecaptchaController;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecaptchaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public RecaptchaController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpPost("Captcha")]
        public async Task<IActionResult> GetreCaptchaResponse([FromBody] reCaptchaRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserResponse))
            {
                return BadRequest("Invalid reCAPTCHA request.");
            }

            var reCaptchaSecretKey = _configuration["reCaptcha:SecretKey"];
            if (string.IsNullOrEmpty(reCaptchaSecretKey))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "reCAPTCHA secret key is not configured.");
            }

            try
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"secret", reCaptchaSecretKey},
            {"response", request.UserResponse}
        });

                var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<reCaptchaResponse>();
                    return Ok(result?.Success ?? false);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"reCAPTCHA verification failed: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for further analysis
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }

        public class reCaptchaRequest
        {
            public string UserResponse { get; set; }
        }

        public class reCaptchaResponse
        {
            public bool Success { get; set; }
            public string[] ErrorCodes { get; set; }
        }

    }
}
