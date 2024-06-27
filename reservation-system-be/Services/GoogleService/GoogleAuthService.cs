using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace reservation_system_be.Services.GoogleService
{
    public class GoogleAuthService
    {
        private readonly IConfiguration _config;
        private readonly ICustomerService _customerService;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(ICustomerService customerService, IConfiguration config, ILogger<GoogleAuthService> logger)
        {
            _customerService = customerService;
            _config = config;
            _logger = logger;
        }

        public string GetGoogleAuthorizationUrl(string redirectUrl)
        {
            var clientId = _config["GoogleAuth:ClientId"];
            return $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}&redirect_uri={redirectUrl}&scope=email%20profile";
        }

        public async Task<AuthDto> HandleGoogleCallbackAsync(string email)
        {
            try
            {
                _logger.LogInformation("Handling Google callback with email: {email}", email);

                // Check if credential is null or empty
                if (string.IsNullOrEmpty(email))
                {
                    _logger.LogWarning("Email is null or empty");
                    throw new ArgumentException("Email is null or empty");
                }


                // Check if user already exists
                var user = await _customerService.GetCustomerByEmail(email);
                if (user != null)
                {
                    _logger.LogInformation("User found. Authenticating user...");

                    // Authenticate user and generate JWT token
                    var googleDTO = new GoogleDTO { Email = user.Email };
                    var token = GenerateJWTToken(googleDTO);
                    var encryptedCustomerId = EncryptionHelper.Encrypt(user.Id);

                    return new AuthDto
                    {
                        token = token,
                        id = encryptedCustomerId,
                        status = user.Status
                    };
                }
                else
                {
                    _logger.LogInformation("User not found. Creating new user...");

                    // Create new user
                    var newCustomer = new Customer
                    {
                        Email = email,
                        // Add other necessary fields here
                    };
                    await _customerService.AddCustomer(newCustomer);

                    // Generate JWT token for new user
                    var googleDTO = new GoogleDTO { Email = newCustomer.Email };
                    var token = GenerateJWTToken(googleDTO);
                    var encryptedCustomerId = EncryptionHelper.Encrypt(newCustomer.Id);

                    return new AuthDto
                    {
                        token = token,
                        id = encryptedCustomerId,
                        status = newCustomer.Status
                    };
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException occurred in HandleGoogleCallbackAsync");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in HandleGoogleCallbackAsync");
                throw new InvalidOperationException("An error occurred while handling the Google callback", ex);
            }
        }

       

        
        private string GenerateJWTToken(GoogleDTO googleDTO)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, googleDTO.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "customer")
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }
}
