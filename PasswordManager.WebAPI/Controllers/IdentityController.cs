using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PasswordManager.WebAPI.Features.Identity.Models;
using Microsoft.Extensions.Options;
using PasswordManager.WebAPI.Models.Identity;
using PasswordManager.WebAPI.Services;

namespace PasswordManager.WebAPI.Controllers
{
    public class IdentityController : ApiControllerBase
    {
        private readonly AppSettings appSettings;
        private readonly IIdentityService identityService;

        public IdentityController(IOptions<AppSettings> appSettings, IIdentityService identityService)
        {
            this.appSettings = appSettings.Value;
            this.identityService = identityService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.EmailAddress == "admin@admin" && user.Password == "admin")
            {
                var tokenString = identityService.GenerateJwtToken("1", user.EmailAddress, appSettings.Secret);

                return Ok(new LoginResponseDTO { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}
