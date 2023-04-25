using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services;
using Services.Abstraction.Data;
using Services.Auth;
using Services.Cryptography;

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

            var passwords = (await _passwordService.GetAllByUserIdAsync(userGuid)).ToArray() ?? Array.Empty<PasswordDTO>();

            foreach (var password in passwords)
            {
                password.PasswordDecrypted = await EncryptionService.DecryptAsync(Convert.FromBase64String(password.PasswordEncrypted),
                    JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims()));
            }

            return Ok(passwords);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var password = await _passwordService.GetByIdAsync(userGuid, guid);

            //password.PasswordDecrypted = password.PasswordEncrypted;

            password.PasswordDecrypted = await EncryptionService.DecryptAsync(Convert.FromBase64String(password.PasswordEncrypted),
                JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims()));

            return Ok(password);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PasswordDTO passwordDTO)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            passwordDTO.PasswordEncrypted = Convert.ToBase64String(await EncryptionService.EncryptAsync(passwordDTO.PasswordDecrypted,
                JwtService.GetUserPasswordFromClaims(HttpContext.GetUserClaims())));

            var password = await _passwordService.CreateAsync(userGuid, passwordDTO);

            return Ok(password);
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> Put(Guid guid, [FromBody] PasswordDTO passwordDTO)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            passwordDTO.PasswordEncrypted = Convert.ToBase64String(await EncryptionService.EncryptAsync(passwordDTO.PasswordDecrypted,
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

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid[] guids)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            foreach (var guid in guids)
            {
                await _passwordService.DeleteAsync(userGuid, guid);
            }

            return Ok();
        }

        [HttpPost("generator")]
        public IActionResult Generate(PasswordGeneratorSettingsDTO generatorSettings)
        {
            var response = new PasswordGeneratorSettingsResponseDTO();
            response.Password = PasswordGeneratorService.GeneratePassword(generatorSettings);

            return Ok(response);
        }
    }
}
