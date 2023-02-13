using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Data;
using Services.Auth;
using Services.Cryptography;
using System.Text;

namespace PasswordManager.WebAPI.Controllers
{
    [Authorize]
    public class PasswordsController : ApiControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordsController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var passwords = await _passwordService.GetAllByUserIdAsync(userGuid);

            // TODO: unnecessary for that page, or change, because in passwords value is not changed 
            //foreach (var password in passwords)
            //{
            //    //password.PasswordDecrypted = password.PasswordEncrypted;
            //    password.PasswordDecrypted = await EncryptionService.DecryptAsync(Encoding.Unicode.GetBytes(password.PasswordEncrypted),
            //        JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims()));
            //}

            return Ok(passwords);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var password = await _passwordService.GetByIdAsync(userGuid, guid);

            //password.PasswordDecrypted = password.PasswordEncrypted;

            password.PasswordDecrypted = await EncryptionService.DecryptAsync(Encoding.Unicode.GetBytes(password.PasswordEncrypted),
                JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims()));

            return Ok(password);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PasswordDTO passwordDTO)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            //passwordDTO.PasswordEncrypted = passwordDTO.PasswordDecrypted;

            passwordDTO.PasswordEncrypted = Encoding.Unicode.GetString(await EncryptionService.EncryptAsync(passwordDTO.PasswordDecrypted,
                JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims())));

            var password = await _passwordService.CreateAsync(userGuid, passwordDTO);

            return Ok(password);
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> Put(Guid guid, [FromBody] PasswordDTO passwordDTO)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            //passwordDTO.PasswordEncrypted = passwordDTO.PasswordDecrypted;

            passwordDTO.PasswordEncrypted = Encoding.Unicode.GetString(await EncryptionService.EncryptAsync(passwordDTO.PasswordDecrypted,
                JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims())));

            var password = await _passwordService.UpdateAsync(userGuid, passwordDTO);

            return Ok(password);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            await _passwordService.DeleteAsync(userGuid, guid);

            return Ok();
        }
    }
}
