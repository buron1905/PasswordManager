using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Services.Abstraction;
using Models.DTOs;
using Services.Abstraction.Auth;

namespace PasswordManager.WebAPI.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly AppSettings _appSettings;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(ILoggerManager loggerManager, IOptions<AppSettings> appSettings, IAuthService authService, IJwtService jwtService)
        {
            _logger = loggerManager;
            _appSettings = appSettings.Value;
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);

            if (response == null)
                return Unauthorized();

            setTokenCookie(response.JweToken!);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);

            if (response == null)
                return Unauthorized();

            setTokenCookie(response.JweToken!);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(/*[FromBody]*/ RefreshAccessTokenRequestDTO refreshAccessTokenRequestDTO)
        {
            var token = refreshAccessTokenRequestDTO.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _authService.RefreshTokenAsync(token);

            if (response == null)
                return Unauthorized();

            setTokenCookie(response.JweToken!);

            return Ok(response);
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.JweTokenMinutesTTL)
            };
            Response.Cookies.Append("token", token, cookieOptions);
        }

    }
}
