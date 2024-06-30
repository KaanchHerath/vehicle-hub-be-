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
                Body = WelcomeMail(customer.Email)
            };

            await _emailService.SendEmailAsync(mailRequest);


            return "Customer registered successfully!";

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
                id = encryptedCustomerId,
                status = user.Status
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
                 new Claim(ClaimTypes.Role, "customer")  // Adding the role claim here
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),//token expiration time
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

            // Generate custom HTML email content
            string emailContent = PasswordResetEmail(customer.Email, otp);

            // Send the custom email
            await _emailService.SendEmailAsync(new MailRequest
            {
                ToEmail = customer.Email,
                Subject = "Password Reset OTP",
                Body = emailContent
            });

            return "Password reset OTP sent.";

        }

        private string PasswordResetEmail(string customerEmail, string otp)
        {
            string response = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8' />
    <title>Password Reset OTP - VehicleHub</title>
</head>
<body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
    <div style='text-align: center; margin-bottom: 20px'>
        <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
    </div>
    <div style='background-color: #ffffff; padding: 50px 30px 10px 30px; border-radius: 10px; margin: 20px auto; max-width: 500px;'> <!-- Adjusted width to 500px -->
        <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Password Reset OTP</h1>
        <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Dear Valued Customer,</h2>
        <p style='color: #888888; text-align: center; font-size: 14px; margin-top: 5px;'>{DateTime.Now.ToString("MMM dd, yyyy")}</p>
        <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>You have requested a password reset for your VehicleHub account.</p>
        <p style='color: #000000; text-align: left; margin-top: 20px;'>Your OTP (One-Time Password) for password reset is: <strong>{otp}</strong></p>
        <p style='color: #000000; text-align: left; margin-top: 20px;'>Please use this OTP within the next 5 minutes to reset your password.</p>
        <p style='color: #000000; text-align: left; margin-top: 20px;'>If you did not request this password reset, please ignore this email.</p>
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
