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


namespace reservation_system_be.Services.CustomerAuthServices

{
    public class CustomerAuthService
    {
       
        private readonly ICustomerService _customerService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public CustomerAuthService(ICustomerService customerService, IEmailService emailService, IConfiguration config)
        {
           
            _customerService = customerService;
            _emailService = emailService;
            _config = config;
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

        public async Task<AuthDto> Login(CustomerAuthDTO customer)
        {
            if (string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            //Find user by Email
            var user = await _customerService.GetCustomerByEmail(customer.Email);

            //Verify user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(customer.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            //Generate JWT token
            var token = GenerateJWTToken(customer);
            var encryptedCustomerId = EncryptionHelper.Encrypt(user.Id);


            var authdto = new AuthDto
            {
                token = token,
                id = encryptedCustomerId
            };

            return authdto;
            
        }

        private string GenerateJWTToken(CustomerAuthDTO customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("Jwt:Key").Value!.PadRight(128, '\0')));
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
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

            customer.Password = HashPassword(newPassword); // Assume you have a method to hash passwords
            customer.PasswordResetOtp = null;
            customer.OtpExpires = null;

            await _customerService.UpdateCustomer(customer.Id, customer);

            return "Password reset successfully.";
        }

        private string HashPassword(string password)
        {
            // Implement password hashing here, e.g., using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
