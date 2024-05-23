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



namespace reservation_system_be.Services.CustomerAuthServices

{
    public class CustomerAuthService
    {
       
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public CustomerAuthService(DataContext context, IConfiguration config)
        {
           
            _context = context;
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
            if(_context.Customers.Any(u => u.Email == customer.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            //Hash the password
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            var newCustomer = new Customer
            {
                Email = customer.Email,
                Password = customer.Password,
                // Assign other properties as needed
            };

            //Add user to database
            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return "User registered successfully";
        }

        public async Task<string> Login(CustomerAuthDTO customer)
        {
            if (string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            //Find user by Email
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.Email == customer.Email);

            //Verify user exists and password is correct
            if(user == null || !BCrypt.Net.BCrypt.Verify(customer.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            //Generate JWT token
            var token = GenerateJWTToken(customer);

            return token;
            
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
            var user = _context.Customers.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Implement password recovery logic here

            // user.PasswordResetToken = CreateRandomToken();
            // user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return "You may now reset your password.";
        }

    }
}
