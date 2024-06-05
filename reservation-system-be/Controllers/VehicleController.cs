﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VehicleDto>>> GetAllVehicles()
        {
            {
                var vehicles = await _vehicleService.GetAllVehicles();
                return Ok(vehicles);
            }
        }
        

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetVehicle(int id)
        {
            try
            {
                return await _vehicleService.GetVehicle(id);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle(Vehicle vehicle)
        {
            return await _vehicleService.CreateVehicle(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Vehicle>> UpdateVehicle(int id, Vehicle vehicle)
        {
            try
            {
                return await _vehicleService.UpdateVehicle(id, vehicle);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            try
            {
                await _vehicleService.DeleteVehicle(id);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("vehicles/search")]
        public async Task<ActionResult<List<VehicleResponse>>> SearchVehicle(String search)
        {
            try
            {
                var vehicles = await _vehicleService.SearchVehicle(search);
                return Ok(vehicles);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("alldata")]
        public async Task<ActionResult<List<VehicleResponse>>> GetAllVehiclesDetails()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesDetails();
                return Ok(vehicles);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("vehicles/filter")]
        public async Task<ActionResult<List<VehicleResponse>>> FilterVehicles(int? vehicleTypeId, int? vehicleMakeId, int? seatingCapacity, float? depositAmount)
        {
            try
            {
                var vehicles = await _vehicleService.FilterVehicles(vehicleTypeId, vehicleMakeId, seatingCapacity, depositAmount);
                if (vehicles == null || vehicles.Count == 0)
                {
                    return NotFound("No vehicles match the provided criteria.");
                }
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}