using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.EmailServices;
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
        private readonly IEmailService _emailService;


        public GoogleAuthService(ICustomerService customerService, IConfiguration config, ILogger<GoogleAuthService> logger, IEmailService emailService)
        {
            _customerService = customerService;
            _config = config;
            _logger = logger;
            _emailService = emailService;
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

                    // Send welcome email to the new customer
                    await SendWelcomeEmailAsync(user.Email);

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

        private async Task SendWelcomeEmailAsync(string customerEmail)
        {
            var mailRequest = new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "Welcome to VehicleHub!",
                Body = WelcomeMail(customerEmail)
            };

            try
            {
                await _emailService.SendEmailAsync(mailRequest);
                _logger.LogInformation("Welcome email sent to {customerEmail}", customerEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send welcome email to {customerEmail}", customerEmail);
                // Handle exception as needed
            }
        }

        private string WelcomeMail(string customerEmail)
        {
            string response = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8' />
    <title>Welcome to VehicleHub</title>
</head>
<body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
    <div style='text-align: center; margin-bottom: 20px'>
        <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
    </div>
    <div style='background-color: #ffffff; padding: 50px 30px 10px 30px; border-radius: 10px; margin: 20px auto; max-width: 500px;'> <!-- Adjusted width to 500px -->
        <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Welcome to VehicleHub!</h1>
        <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Dear Valued Customer,</h2>
        <p style='color: #888888; text-align: center; font-size: 14px; margin-top: 5px;'>{DateTime.Now.ToString("MMM dd, yyyy")}</p>
        <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>Thank you for registering with VehicleHub. We are thrilled to have you with us!</p>
        <p style='color: #000000; text-align: left; margin-top: 20px;'>At VehicleHub, we are dedicated to providing you with exceptional service and a wide range of top-quality vehicles. Whether you need a vehicle for a day, a week, or longer, we have the perfect option for you.</p>
        <p style='color: #000000; text-align: left; margin-top: 20px;'>To get started, visit our <a href='http://localhost:3000'>website</a> and explore our latest vehicle options.</p>
        <p style='color: #000000; text-align: left; margin-top: 20px;'>We are here to assist you with any queries and ensure you have a seamless experience.</p>
        <p style='color: #000000; text-align: left; margin-bottom: 5px;'>Warm regards,</p>
        <p style='color: #000000; text-align: left; margin-top: 5px;'><strong>VehicleHub Team</strong></p>
        <p style='padding: 10px; margin-top: 40px; text-align: center;'>Contact us: <a href='mailto:vehiclehub01@gmail.com'>vehiclehub01@gmail.com</a> | <a href='tel:+94771234567'>+94 77 123 4567</a></p>
    </div>
    <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
        <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. {DateTime.Now.Year}</strong></p>
        <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
    </div>
</body>
</html>";

            return response;
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
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }
}
