using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.AdditionalFeaturesServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalFeaturesController : ControllerBase
    {
        private readonly IAdditionalFeaturesService _additionalFeaturesService;

        public AdditionalFeaturesController(IAdditionalFeaturesService additionalFeaturesService) 
        {
            _additionalFeaturesService = additionalFeaturesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdditionalFeaturesDto>>> GetAllAdditionalFeatures()
        {
            var additionalFeatures = await _additionalFeaturesService.GetAllAdditionalFeatures();
            return Ok(additionalFeatures);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdditionalFeaturesDto>> GetAdditionalFeatures(int id)
        {
            try
            {
                return await _additionalFeaturesService.GetAdditionalFeatures(id);
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddAdditionalFeatures(CreateVehicleModelDto createVehicleModelDto)
        { 
            try
            {
                await _additionalFeaturesService.AddAdditionalFeatures(createVehicleModelDto);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAdditionalFeatures(int id, CreateVehicleModelDto createVehicleModelDto)
        {
            try
            {
                await _additionalFeaturesService.UpdateAdditionalFeatures(id, createVehicleModelDto);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdditionalFeatures(int id)
        {
            try
            {
                await _additionalFeaturesService.DeleteAdditionalFeatures(id);
                return Ok();
            }
            catch (DataNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
