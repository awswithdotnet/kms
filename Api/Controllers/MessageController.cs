using System;
using System.Threading.Tasks;
using Abstractions;
using Crypto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> ReceiveMessage(EncryptionPackage encryptionPackage){

            if(encryptionPackage == null || encryptionPackage.CipherText == null || encryptionPackage.EncryptedKey == null){
                return BadRequest();
            }

            IDecrypter decrypter = new AESDecrypter();

            string plainText = await decrypter.Decrypt(encryptionPackage);

            Console.WriteLine("Decrypted:" + plainText);

            return Ok(plainText);
        }
    }
}