using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Helper;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        [HttpGet("encrypt/{Id}")]
        public IActionResult Encrypt(int Id)
        {
            try
            {
                string encryptedText = EncryptionHelper.Encrypt(Id);
                return Ok(new { EncryptedText = encryptedText });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error encrypting user ID", Error = ex.Message });
            }
        }

        [HttpGet("decrypt/{cipherText}")]
        public IActionResult Decrypt(string cipherText)
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
