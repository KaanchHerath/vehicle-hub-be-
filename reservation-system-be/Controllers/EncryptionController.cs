using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Helper;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        [HttpGet("encrypt/{userId}")]
        public IActionResult Encrypt(int userId)
        {
            try
            {
                string encryptedText = EncryptionHelper.Encrypt(userId);
                return Ok(new { EncryptedText = encryptedText });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error encrypting user ID", Error = ex.Message });
            }
        }

        [HttpGet("decrypt")]
        public IActionResult Decrypt([FromQuery] string cipherText)
        {
            try
            {
                int decryptedUserId = EncryptionHelper.Decrypt(cipherText);
                return Ok(new { DecryptedUserId = decryptedUserId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error decrypting cipher text", Error = ex.Message });
            }
        }

    }
}
