using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Data;
using Services.Auth;

namespace PasswordManager.WebAPI.Controllers
{
    [Authorize]
    public class SettingsController : ApiControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var settings = await _settingsService.GetSettingsByUser(userGuid);

            return Ok(settings);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SettingsDTO settingsDTO)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var settings = await _settingsService.UpdateAsync(userGuid, settingsDTO);

            return Ok(settings);
        }

    }
}
