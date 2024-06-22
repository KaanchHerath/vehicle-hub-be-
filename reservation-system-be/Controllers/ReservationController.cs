using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetAllReservations()
        {
            return await _reservationService.GetAllReservations();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            try
            {
                return await _reservationService.GetReservation(id);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            return await _reservationService.CreateReservation(reservation);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Reservation>> UpdateReservation(int id, Reservation reservation)
        {
            try
            {
                return await _reservationService.UpdateReservation(id, reservation);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            try
            {
                await _reservationService.DeleteReservation(id);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }   
    }
}
