using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Services.Abstraction;
using Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using PasswordManager.WebAPI.Helpers;
using Services.Data;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Auth;
using Services.Auth;
using Services.TMP;

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

            setTokenCookie(response.RefreshToken!);
            
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);

            if (response == null)
                return Unauthorized();
            
            setTokenCookie(response.RefreshToken!);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("token-is-valid")]
        public IActionResult TokenIsValid(string token)
        {
            bool response = _authService.TokenIsValid(token);
            
            return Ok(new { isValid = response });
        }
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string? password = null;
            if (token != null)
            {
                password = _jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey, false);
            }
            if (password is null)
                return Unauthorized();
                
            var response = await _authService.RefreshToken(refreshToken, password);


            if (response == null)
                return Unauthorized();

            setTokenCookie(response.RefreshToken!);
            
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(/*[FromBody]*/ RevokeTokenRequestDTO revokeTokenRequestDTO)
        {
            // accept refresh token from body request otherwise from cookie
            var token = revokeTokenRequestDTO.Token ?? Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            bool response = await _authService.RevokeToken(token);

            return Ok(new { isRevoked = response });
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(_appSettings.RefreshTokenDaysTTL)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        
    }
}
