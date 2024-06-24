using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Helper;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;


namespace reservation_system_be.Services.CustomerAuthServices

{
    public class CustomerAuthService
    {
        private readonly DataContext _context;

        private readonly ICustomerService _customerService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExternalLoginService _externalLoginService;
        public CustomerAuthService(DataContext context, ICustomerService customerService, IEmailService emailService, IConfiguration config, IHttpContextAccessor httpContextAccessor, IExternalLoginService externalLoginService)
        {
            _context = context;
            _customerService = customerService;
            _emailService = emailService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _externalLoginService = externalLoginService;
        }
        public async Task<string> Register(CustomerAuthDTO customer)
        {
            //Validate user input
            if (string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.Password))
            {
                throw new ArgumentException("Email and password are required");
            }

            //Check if email already exists
            var existingCustomer = await _customerService.GetCustomerByEmail(customer.Email);
            if (existingCustomer != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            //Hash the password

            customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            var newCustomer = new Customer
            {
                Email = customer.Email,
                Password = customer.Password,

            };

            //Add user to database
            await _customerService.AddCustomer(newCustomer);

            //send welcome email
            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customer.Email,
                Subject = "Welcome to Vehicle Hub",
                Body = "<h1>Welcome!</h1> <br> <p>Thank you for registering with our service. Now you can reserve vehicles whenever you need.</p>"
            };

            await _emailService.SendEmailAsync(mailRequest);


            return "Customer registered successfully!";

        }

        public async Task<AuthDto> Login(CustomerAuthDTO customerAuthDTO)
        {
            if (string.IsNullOrEmpty(customerAuthDTO.Email) || string.IsNullOrEmpty(customerAuthDTO.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            //Find user by Email
            var user = await _customerService.GetCustomerByEmail(customerAuthDTO.Email);
            if (user == null)
            {
                Console.WriteLine("User not found");
                throw new UnauthorizedAccessException("Invalid email");
            }

            if (!user.Status)
            {
                // Account is inactive
                throw new UnauthorizedAccessException("Account is inactive. Please contact support.");
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(customerAuthDTO.Password, user.Password))
            {
                Console.WriteLine("Password verification failed");
                throw new UnauthorizedAccessException("Invalid password");
            }

            //Generate JWT token
            var token = GenerateJWTToken(customerAuthDTO);
            var encryptedCustomerId = EncryptionHelper.Encrypt(user.Id);


            var authdto = new AuthDto
            {
                token = token,
                id = encryptedCustomerId
            };

            return authdto;

        }

        private string GenerateJWTToken(CustomerAuthDTO customerAuthDTO)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(key) || key.Length < 16)
            {
                throw new InvalidOperationException("The JWT key is either missing or too short.");
            }

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!.PadRight(128, '\0')));
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customerAuthDTO.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),//token expiration time
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ForgotPassword(string email)
        {
            var customer = await _customerService.GetCustomerByEmail(email);

            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }
            var otp = GenerateOtp();

            customer.PasswordResetOtp = otp;
            customer.OtpExpires = DateTime.UtcNow.AddMinutes(5); // OTP expires in 5 minutes


            await _customerService.UpdateCustomer(customer.Id, customer);

            await _emailService.SendPasswordResetOtpAsync(customer.Email, otp);

            return "Password reset OTP sent.";

        }

        private string GenerateOtp()
        {
            var rng = new Random();
            var otp = rng.Next(100000, 999999).ToString(); // Generate a 6-digit OTP
            return otp;
        }

        public async Task<string> VerifyOtp(string otp)
        {
            var customer = await _customerService.GetCustomerByOtp(otp);

            if (customer == null || customer.OtpExpires < DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid or expired OTP.");
            }

            customer.PasswordResetOtp = otp;
            await _customerService.UpdateCustomer(customer.Id, customer);

            return "OTP verified successfully";

        }

        public async Task<string> ResetPassword(string otp, string newPassword)
        {
            var customer = await _customerService.GetCustomerByOtp(otp);

            if (customer == null || customer.OtpExpires < DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid or expired OTP.");
            }

            customer.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            customer.PasswordResetOtp = null;
            customer.OtpExpires = null;

            //await _customerService.UpdateCustomer(customer.Id, customer);
                _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Password for user {customer.Email} has been reset.");
            return "Password reset successfully.";
        }

      
        public async Task<string> ResetPasswordProfile(ProfilePasswordDTO profilePasswordDTO)
        {
            var user = await _customerService.GetCustomerById(profilePasswordDTO.Id);
            if (user == null)
            {
                throw new InvalidOperationException("Customer not found");
            }



            // Verify current password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(profilePasswordDTO.CurrentPassword, user.Password))
            {
                throw new InvalidOperationException("Current password is incorrect");
            }

            // Update the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(profilePasswordDTO.NewPassword);
            //user.Password = customer.NewPassword;
            _context.Customers.Update(user);
            await _context.SaveChangesAsync();

            return "Password has been reset successfully";
        }

        public string Logout()
        {
            // Clear the user's session data
            _httpContextAccessor.HttpContext?.Session.Clear();
            return "Logged out successfully!";
        }

        public async Task DeactivateCustomer(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                throw new DataNotFoundException("Customer not found");
            }

            // Set status to inactive instead of deleting the record
            customer.Status = false;
            await _customerService.UpdateCustomer(id, customer);
        }

        public async Task ReactivateCustomer(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
            {
                throw new DataNotFoundException("Customer not found");
            }

            // Set status to inactive instead of deleting the record
            customer.Status = true;
            await _customerService.UpdateCustomer(id, customer);
        }


    }
}
