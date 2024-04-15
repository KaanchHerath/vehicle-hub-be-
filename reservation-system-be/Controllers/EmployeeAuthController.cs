using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;


        public EmployeeAuthController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Employee employee)
        {
            //Find user by username
            var user =  _context.Employees.FirstOrDefault(u => u.Email == employee.Email);

            //Verify user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(employee.Password, employee.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            //Generate JWT token
            var tokenstring = GenerateJWTToken(employee);

            //Return token to the client
            return Ok(new { token = tokenstring });
        }

        private string GenerateJWTToken(Employee employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, employee.Role)
            };

            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),//token expiration time
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
