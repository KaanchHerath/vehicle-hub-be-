using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Data;
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
        public async Task<ActionResult<List<AdditionalFeatures>>> GetAllAdditionalFeatures()
        {
            return await _additionalFeaturesService.GetAllAdditionalFeatures();
        }

        [HttpGet]
        public async Task<ActionResult<AdditionalFeatures>> GetAdditionalFeatures(int id)
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
        public async Task<ActionResult<AdditionalFeatures>> AddAdditionalFeatures(AdditionalFeatures additionalFeatures)
        {
            return await _additionalFeaturesService.AddAdditionalFeatures(additionalFeatures);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AdditionalFeatures>> UpdateAdditionalFeatures(int id, AdditionalFeatures additionalFeatures)
        {
            try
            {
                return await _additionalFeaturesService.UpdateAdditionalFeatures(id, additionalFeatures);
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
