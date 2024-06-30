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
using reservation_system_be.Services.CustomerAuthServices;
using reservation_system_be.DTOs;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Azure;
using Microsoft.AspNetCore.Authorization;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAuthController : ControllerBase
    {
        private readonly CustomerAuthService _customerAuthService;


        public CustomerAuthController(CustomerAuthService customerAuthService)
        {
            _customerAuthService = customerAuthService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerAuthDTO customer)
        {
            try
            {
                var result = await _customerAuthService.Register(customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerAuthDTO customerAuthDTO)
        {
            try
            {
                var token = await _customerAuthService.Login( customerAuthDTO);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        
        
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var result = await _customerAuthService.ForgotPassword(email);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(string otp)
        {
            try
            {
                await _customerAuthService.VerifyOtp(otp);
                return Ok("OTP verified successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string otp, string password)
        {
            try
            {
                var result = await _customerAuthService.ResetPassword(otp, password);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpPost("ResetPasswordProfile")]
        public async Task<IActionResult> ResetPasswordProfile([FromBody] ProfilePasswordDTO profilePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _customerAuthService.ResetPasswordProfile(profilePasswordDTO);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }  
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var result = _customerAuthService.Logout();
            return Ok(result);
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateCustomer(int id)
        {
            try
            {
                await _customerAuthService.DeactivateCustomer(id);
                return Ok(new { message = "Customer deactivated successfully" });
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("reactivate/{id}")]
        public async Task<IActionResult> ReactivateCustomer(int id)
        {
            try
            {
                await _customerAuthService.ReactivateCustomer(id);
                return Ok(new { message = "Customer Reactivated successfully" });
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }





    }


}

