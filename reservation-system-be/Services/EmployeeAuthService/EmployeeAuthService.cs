using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.EmployeeServices;
using Stripe;


namespace reservation_system_be.Services.EmployeeAuthService
{
    public class EmployeeAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;


        public EmployeeAuthService(DataContext context, IConfiguration config, IEmployeeService employeeService, IEmailService emailService)
        {

            _context = context;
            _config = config;
            _employeeService = employeeService;
            _emailService = emailService;
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

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = employee.Email,
                Subject = "Welcome to Vehicle Hub",
                Body = "<h1>Welcome!</h1> <br> <p>Thank you for registering with our service. Now you can reserve vehicles whenever you need.</p>"
            };

            await _emailService.SendEmailAsync(mailRequest);

            //Add user to database
            await _employeeService.AddEmployee(employee);

            return "Employee registered successfully";
        }

        public async Task<AuthDto> Login(Employee employee)
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
            var token = GenerateJWTToken(user);
            var employeeId = user.Id;

            var authdto = new AuthDto
            {
                token = token,
                id = EncryptionHelper.Encrypt(employeeId)
            };

            return authdto;

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
