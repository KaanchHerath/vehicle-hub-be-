using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using reservation_system_be.Services.EmployeeAuthService;
using Microsoft.AspNetCore.Authorization;
using reservation_system_be.Services.CustomerAuthServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAuthController : ControllerBase
    {
        private readonly EmployeeAuthService _employeeAuthService;


        public EmployeeAuthController(EmployeeAuthService employeeAuthService)
        {
            _employeeAuthService = employeeAuthService;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(Employee employee)
        {
            try
            {
                var result = await _employeeAuthService.Register(employee);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Employee employee)
        {
            try
            {
                var token = await _employeeAuthService.Login(employee);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] EmployeePasswordDTO employeePasswordDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _employeeAuthService.ResetPassword(employeePasswordDTO);
            if (result == "Password has been reset successfully")
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPost("logout")]

        public IActionResult Logout()
        {
            var result = _employeeAuthService.Logout();
            return Ok(result);
        }

        [HttpPost("deactivate/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            try
            {
                await _employeeAuthService.DeactivateEmployee(id);
                return Ok(new { message = "Employee deactivated successfully" });
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
