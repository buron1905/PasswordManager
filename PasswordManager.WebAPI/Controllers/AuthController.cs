using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Services.Abstraction;
using Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using PasswordManager.WebAPI.Helpers;

namespace PasswordManager.WebAPI.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly AppSettings _appSettings;
        private readonly IAuthService _authService;


        public AuthController(ILoggerManager loggerManager, IOptions<AppSettings> appSettings, IAuthService authService)
        {
            _logger = loggerManager;
            _appSettings = appSettings.Value;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            
            if (response == null)
                return Unauthorized();

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);

            if (response == null)
                return Unauthorized();

            return Ok(response);
        }

        [HttpGet("token-is-valid")]
        public IActionResult TokenIsValid(string token)
        {
            bool response = _authService.TokenIsValid(token);
            
            return Ok(new { isValid = response });
        }
    }
}
