using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;


namespace reservation_system_be.Services.EmployeeAuthService
{
    public class EmployeeAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public EmployeeAuthService(DataContext context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }
        public async Task<string> Register(Employee employee)
        {
            //Validate user input
            if (string.IsNullOrEmpty(employee.Email) || string.IsNullOrEmpty(employee.Password))
            {
                throw new ArgumentException("Email and password are required");
            }


            //Check if username already exists
            if (_context.Employees.Any(u => u.Email == employee.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            //Hash the password
            employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

            var newEmployee = new Employee
            {
                Email = employee.Email,
                Password = employee.Password,
                // Assign other properties as needed
            };

            //Add user to database
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();

            return "Employee registered successfully";
        }

        public async Task<string> Login(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Email) || string.IsNullOrEmpty(employee.Password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            //Find user by Email
            var user = await _context.Employees.FirstOrDefaultAsync(u => u.Email == employee.Email);

            //Verify user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(employee.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            //Generate JWT token
            var token = GenerateJWTToken(employee);

            return token;

        }

        private string GenerateJWTToken(Employee employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("Jwt:Key").Value!.PadRight(128, '\0')));
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
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
    }
}
