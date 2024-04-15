using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.Models;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;


        public CustomerAuthController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Customer customer)
        {
            //Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Check if username already exists
            if (_context.Customers.Any(u => u.Email == customer.Email))
            {
                return Conflict("Email already exists");
            }

            customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            //Add user to database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Customer customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.Email) || string.IsNullOrEmpty(customer.Password))
            {
                return BadRequest("Invalid credentials");
            }

            //Find user by username
            var user = _context.Customers.FirstOrDefault(u => u.Email == customer.Email);

            //Verify user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(customer.Password, user.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            //Generate JWT token
            var tokenstring = GenerateJWTToken(user);

            //Return token to the client
            return Ok(new { token = tokenstring });

        }

        private string GenerateJWTToken(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
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
        
        [HttpPost("Forgot password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (_context == null)
            {
                // Handle the situation where the database context is not initialized.
                return StatusCode(500, "Internal server error");
            }

            //find user by username
            var user = _context.Customers.FirstOrDefault(u => u.Email == email);

            //Check if user exists
            if (user == null)
            {
                return NotFound("User not found");
            }

            //Implement password recovery logic here

           // user.PasswordResetToken = CreateRandomToken();
            //user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password.");
            //write a function to create random token
        }
    }
}

