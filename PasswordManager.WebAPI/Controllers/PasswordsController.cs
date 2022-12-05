using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Data;
using Services.Auth;

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

            return Ok(passwords);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(string guid)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PasswordDTO passwordDTO)
        {
            //var password = await _passwordService.CreateAsync();

            throw new NotImplementedException();
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> Put(string guid, [FromBody] PasswordDTO passwordDTO)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> Delete(string guid)
        {
            throw new NotImplementedException();
        }
    }
}
