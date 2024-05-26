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
        public async Task<IEnumerable<AdditionalFeaturesDto>> GetAllAdditionalFeatures()
        {
            return await _additionalFeaturesService.GetAllAdditionalFeatures();
        }

        [HttpGet("{id}")]
        public async Task<AdditionalFeaturesDto> GetAdditionalFeatures(int id)
        {
            return await _additionalFeaturesService.GetAdditionalFeatures(id);
        }

        [HttpPost]
        public async Task<ActionResult<AdditionalFeatures>> AddAdditionalFeatures(AdditionalFeatures additionalFeatures)
        {
            var newAdditionalFeatures = await _additionalFeaturesService.AddAdditionalFeatures(additionalFeatures);
            return CreatedAtAction(nameof(GetAdditionalFeatures), new { id = newAdditionalFeatures.Id }, newAdditionalFeatures);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdditionalFeatures(int id, AdditionalFeatures additionalFeatures)
        {
            if (id != additionalFeatures.Id)
            {
                return BadRequest();
            }
            await _additionalFeaturesService.UpdateAdditionalFeatures(id, additionalFeatures);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdditionalFeatures(int id)
        {
            await _additionalFeaturesService.DeleteAdditionalFeatures(id);
            return NoContent();
        }
       
    }
}
