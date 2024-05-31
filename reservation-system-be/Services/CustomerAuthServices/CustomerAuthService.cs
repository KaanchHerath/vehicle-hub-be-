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
            var mailRequest = new MailRequest
            {
                ToEmail = customer.Email,
                Subject = "Welcome to Vehicle Hub",
                Body = "<h1>Welcome!</h1> <br> <p>Thank you for reistering with our service. Now you can reserve vehicles whenever you need.</p>"
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
            var customerId = user.Id;

            var authdto = new AuthDto
            {
                token = token,
                id = customerId
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
            var user = await _customerService.GetCustomerByEmail(email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Implement password recovery logic here

            // user.PasswordResetToken = CreateRandomToken();
            // user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _customerService.UpdateCustomer(user.Id, user);

            return "You may now reset your password.";
        }

    }
}
