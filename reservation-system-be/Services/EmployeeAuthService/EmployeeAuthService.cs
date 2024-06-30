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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeAuthService(DataContext context, IConfiguration config, IEmployeeService employeeService, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _config = config;
            _employeeService = employeeService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string WelcomeMail(string employeeEmail, string resetLink)
        {
            string response = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8' />
    <title>Welcome to VehicleHub</title>
</head>
<body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
    <div style='width:100%;background-color:#f4f4f4;text-align:center;margin:10px;padding:10px;font-family:Arial, sans-serif;'>
        <div style='background-color:#283280;color:#ffffff;padding:10px;'>
            <h1>VehicleHub</h1>
        </div>
        <div style='margin:20px;text-align:left;'>
            <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 150px; height: auto; display: block; margin: auto;'/>
            <h2 style='text-align:center;'>Welcome to Vehicle Hub!</h2>
            <p>Thank you for working with our company. You are now an employee of Vehicle Hub.</p>
            <p>Your default password is: <strong>NavodhVehicleHub789</strong>.</p>
            <p>Please <a href='{resetLink}'>click here</a> and login to reset your password immediately.</p>
            <p style='margin-top:20px;'>We appreciate your commitment and look forward to working with you.</p>
            <p>Best regards,</p>
            <p><strong>VehicleHub Team</strong></p>
        </div>
        <div style='background-color:#283280;color:#ffffff;padding:10px;margin-top:20px;text-align:center;'>
            <p>Contact us: info@vehiclehub.com | (123) 456-7890</p>
            <p>1234 Main St, Anytown, USA</p>
        </div>
        <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
            <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. {DateTime.Now.Year}</strong></p>
            <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
        </div>
    </div>
</body>
</html>";

            return response;
        }

        public async Task<string> Register(Employee employee)
        {
            const string defaultPassword = "NavodhVehicleHub789"; // Define a default password
            //Validate user input
            if (string.IsNullOrEmpty(employee.Email))
            {
                throw new ArgumentException("Email is required");
            }


            //Check if username already exists
            if (_context.Employees.Any(u => u.Email == employee.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            //Hash the password
            employee.Password = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

            //Add user to database
            await _employeeService.AddEmployee(employee);

            // Generate password reset token
            var token = GeneratePasswordResetToken(employee);
            var resetLink = $"http://localhost:3000/admin-login";


            MailRequest mailRequest = new MailRequest
            {
                ToEmail = employee.Email,
                Subject = "Welcome to Vehicle Hub",
                Body = WelcomeMail(employee.Email, resetLink)
            };

            await _emailService.SendEmailAsync(mailRequest);

            return "Employee registered successfully";
        }

        private string GeneratePasswordResetToken(Employee employee)
        {
            // Generate a unique token for the password reset link
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, employee.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

            var roles = DetermineUserRoles(user);

            //Generate JWT token
            var token = GenerateJWTToken(user, roles);
            var employeeId = user.Id;

            var authdto = new AuthDto
            {
                token = token,
                id = EncryptionHelper.Encrypt(employeeId)
            };

            return authdto;

        }

        private List<string> DetermineUserRoles(Employee user)
        {
            // Example: Determine roles based on user properties or database lookup
            var roles = new List<string>();
            if (user.Role == "Admin")
            {
                roles.Add("admin");
            }

            if (user.Role == "Staff")
            {
                roles.Add("staff");
            }
            // Add more roles as needed based on your application's logic

            return roles;
        }

        private string GenerateJWTToken(Employee employee, List<string>roles)
        {

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                //_config.GetSection("Jwt:Key").Value!.PadRight(256, '\0')));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add roles to claims if roles are provided
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role)); // Add each role as a separate claim
                }
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),//token expiration time
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ResetPassword(EmployeePasswordDTO employeePasswordDTO)
        {
            var user = await _context.Employees.FindAsync(employeePasswordDTO.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User with the provided ID not found");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(employeePasswordDTO.CurrentPassword, user.Password))
            {
                throw new InvalidOperationException("Current password is incorrect");
            }

            // Update the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(employeePasswordDTO.NewPassword);
            _context.Employees.Update(user);
            await _context.SaveChangesAsync();

            return "Password has been reset successfully";
        }

        public string Logout()
        {
            // Clear the user's session data
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.Clear();

                // Remove all cookies
                foreach (var cookie in context.Request.Cookies.Keys)
                {
                    context.Response.Cookies.Delete(cookie);
                }
            }

            return "Logged out successfully!";
        }

        public async Task DeactivateEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                throw new DataNotFoundException("Customer not found");
            }

            // Set status to inactive instead of deleting the record
            employee.Status = false;
            await _employeeService.UpdateEmployee(id, employee);
        }
    }
}
