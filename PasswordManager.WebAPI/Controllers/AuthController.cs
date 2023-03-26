using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Auth;
using Services.Auth;

namespace PasswordManager.WebAPI.Controllers
{
    public class AuthController : ApiControllerBase
    {
        #region Private fields

        private readonly AppSettings _appSettings;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        #endregion

        public AuthController(IOptions<AppSettings> appSettings, IAuthService authService, IJwtService jwtService)
        {
            _appSettings = appSettings.Value;
            _authService = authService;
            _jwtService = jwtService;
        }

        #region Actions

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);

            if (response == null)
                return Unauthorized(new AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid Authentication" });

            if (!response.IsTfaEnabled)
                SetTokenCookie(response.JweToken!);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);

            if (response == null)
                return Unauthorized(new AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid Authentication" });

            SetTokenCookie(response.JweToken!);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshAccessTokenRequestDTO refreshAccessTokenRequestDTO)
        {
            var token = refreshAccessTokenRequestDTO.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _authService.RefreshTokenAsync(token);

            if (response == null)
                return Unauthorized();

            SetTokenCookie(response.JweToken!);

            return Ok(response);
        }

        #region TFA

        [HttpPost("tfa-login")]
        public async Task<IActionResult> LoginTfa([FromBody] LoginTfaRequestDTO requestDTO)
        {
            var response = await _authService.LoginTfaAsync(requestDTO);

            if (response == null)
                return Unauthorized(new AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid Authentication" });

            SetTokenCookie(response.JweToken!);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("tfa-setup")]
        public async Task<IActionResult> GetTfaSetup()
        {
            var claims = HttpContext.GetUserClaims();
            var email = JwtService.GetUserEmailFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);

            var result = await _authService.GetTfaSetup(email, password);

            if (result is null)
                return BadRequest();

            return Ok(result);
        }

        [Authorize]
        [HttpPost("tfa-setup")]
        public async Task<IActionResult> PostTfaSetup([FromBody] TfaSetupDTO tfaSetupDTO)
        {
            var claims = HttpContext.GetUserClaims();
            var userId = JwtService.GetUserGuidFromClaims(claims);
            var email = JwtService.GetUserEmailFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);

            var isValidCode = _authService.ValidateTfaCode(password, tfaSetupDTO.Code);

            if (!isValidCode)
                return BadRequest("Invalid code");

            var response = await _authService.SetTwoFactorEnabledAsync(userId, password);
            var tfaResponse = _authService.GenerateTfaSetupDTO("Password Manager", email!, password!);
            tfaResponse.IsTfaEnabled = true;

            if (response is null)
                return Unauthorized(new AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "" });

            SetTokenCookie(response.JweToken!);

            return Ok(tfaResponse);
        }

        [Authorize]
        [HttpPost("tfa-disable")]
        public async Task<IActionResult> DisableTfaSetup([FromBody] TfaSetupDTO tfaSetupDTO)
        {
            var claims = HttpContext.GetUserClaims();
            var userId = JwtService.GetUserGuidFromClaims(claims);
            var email = JwtService.GetUserEmailFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);

            var isValidCode = _authService.ValidateTfaCode(password, tfaSetupDTO.Code);

            if (!isValidCode)
                return BadRequest("Invalid code");

            await _authService.SetTwoFactorDisabledAsync(userId);
            var tfaResponse = _authService.GenerateTfaSetupDTO("Password Manager", email!, password!);
            tfaResponse.IsTfaEnabled = false;

            return Ok(tfaResponse);
        }

        #endregion TFA

        #endregion

        #region Methods

        private void SetTokenCookie(string token)
        {
            // append cookie with token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.JweTokenMinutesTTL)
            };
            Response.Cookies.Append("token", token, cookieOptions);
        }

        #endregion
    }
}
