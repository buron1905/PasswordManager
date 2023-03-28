using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using Services.Abstraction.Data;
using Services.Auth;

namespace PasswordManager.WebAPI.Controllers
{
    [Authorize]
    public class SyncController : ApiControllerBase
    {
        private readonly ISyncService _syncService;

        public SyncController(ISyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastChangeDate()
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var response = await _syncService.GetLastChangeDateTime(userGuid);
            if (response is null)
                return BadRequest();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SyncRequestDTO requestDTO)
        {
            var userGuid = JwtService.GetUserGuidFromClaims(HttpContext.GetUserClaims());

            var response = await _syncService.SyncAccount(requestDTO);
            if (response is null)
                return BadRequest();

            return Ok(response);
        }

    }
}
