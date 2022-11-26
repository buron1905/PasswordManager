using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;

namespace PasswordManager.WebAPI.Controllers
{
    [Authorize]
    public class PasswordsController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPasswordService _passwordService;

        public PasswordsController(IAuthService authService, IPasswordService passwordService)
        {
            _authService = authService;
            _passwordService = passwordService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string userIdString = HttpContext.Items["userId"].ToString();

            var guid = new Guid(userIdString);

            var passwords = await _passwordService.GetAllByUserIdAsync(guid);

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
