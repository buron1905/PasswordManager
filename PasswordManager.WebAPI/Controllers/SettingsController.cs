using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Data;

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
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SettingsDTO settingsDTO)
        {
            throw new NotImplementedException();
        }
    }
}
