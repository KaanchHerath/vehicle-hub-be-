using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReservationController : ControllerBase
    {
        private readonly ICustomerReservationService _customerReservationService;

        public CustomerReservationController(ICustomerReservationService customerReservationService)
        {
            _customerReservationService = customerReservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllCustomerReservations()
        {
            var customerReservation = await _customerReservationService.GetAllCustomerReservations();
            return Ok(customerReservation);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReservation>> GetCustomerReservation(int id)
        {
            try
            {
                return await _customerReservationService.GetCustomerReservation(id);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerReservation>> CreateCustomerReservation(CreateCustomerReservationDto customerReservationDto)
        {
            return await _customerReservationService.CreateCustomerReservation(customerReservationDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerReservation>> UpdateCustomerReservation(int id, CreateCustomerReservationDto customerReservationDto)
        {
            try
            {
                return await _customerReservationService.UpdateCustomerReservation(id, customerReservationDto);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerReservation(int id)
        {
            try
            {
                await _customerReservationService.DeleteCustomerReservation(id);
                return NoContent();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }   
    }
}
