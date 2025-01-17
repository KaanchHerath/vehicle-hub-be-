﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleInsuranceServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminAndStaffOnly")]
    public class VehicleInsuranceController : ControllerBase
    {
        private readonly IVehicleInsuranceService _vehicleInsuranceService;

        public VehicleInsuranceController(IVehicleInsuranceService vehicleInsuranceService)
        {
            _vehicleInsuranceService = vehicleInsuranceService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleInsuranceDto>>> GetAllVehicleInsurance()
        {
            var vehicleInsurances = await _vehicleInsuranceService.GetAllVehicleInsurances();
            return Ok(vehicleInsurances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleInsuranceDto>> GetSingleVehicleInsurance(int id)
        {
            try
            {
                var vehicleInsurance = await _vehicleInsuranceService.GetSingleVehicleInsurance(id);
                return Ok(vehicleInsurance);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<ActionResult<CreateVehicleInsuranceDto>> AddVehicleInsurance(CreateVehicleInsuranceDto vehicleInsurance)
        {
            var newVehicleInsurance = await _vehicleInsuranceService.CreateVehicleInsurance(vehicleInsurance);
            return CreatedAtAction(nameof(GetSingleVehicleInsurance), new { id = newVehicleInsurance.Id }, newVehicleInsurance);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleInsurance>> UpdateVehicleInsurance(int id, CreateVehicleInsuranceDto vehicleInsurance)
        {
            try
            {
                var updatedVehicleInsurance = await _vehicleInsuranceService.UpdateVehicleInsurance(id, vehicleInsurance);
                return Ok(updatedVehicleInsurance);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleInsurance(int id)
        {
            try
            {
                await _vehicleInsuranceService.DeleteVehicleInsurance(id);
                return NoContent();
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
