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

            if (!response.EmailVerified)
                return Ok(new AuthResponseDTO { IsAuthSuccessful = false, EmailVerified = false, ErrorMessage = "Email is not confirmed." });

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

            //SetTokenCookie(response.JweToken!);

            return Ok(response);
        }

        [HttpPost("email-confirm")]
        public async Task<IActionResult> ConfirmEmail([FromBody] VerifyEmailRequestDTO requestDTO)
        {
            if (requestDTO.Email is null || requestDTO.Token is null)
                return BadRequest("Invalid Email Confirmation Request");

            var confirmed = await _authService.ConfirmEmailAsync(requestDTO.Email, requestDTO.Token);

            if (!confirmed)
                return BadRequest("Invalid Email Confirmation Request");

            return Ok();
        }

        [HttpPost("email-confirm-resend/{email}")]
        public async Task<IActionResult> ResendConfirmEmail(string email)
        {
            if (email is null)
                return BadRequest("Invalid Resend Confirmation Email Request");

            await _authService.ResendConfirmEmail(email);

            return Ok();
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
            var userId = JwtService.GetUserGuidFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);

            var result = await _authService.GetTfaSetup(userId, password);

            if (result is null)
                return BadRequest();

            return Ok(result);
        }

        [Authorize]
        [HttpPost("tfa-setup")]
        public async Task<IActionResult> EnableTfaSetup([FromBody] TfaSetupDTO tfaSetupDTO)
        {
            var claims = HttpContext.GetUserClaims();
            var userId = JwtService.GetUserGuidFromClaims(claims);
            var email = JwtService.GetUserEmailFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);

            var result = await _authService.EnableTfa(userId, password, tfaSetupDTO);

            if (result is null)
                return BadRequest();

            var response = _authService.GetAuthResponse(userId, email, password, true, true, true);
            SetTokenCookie(response.JweToken!);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("tfa-disable")]
        public async Task<IActionResult> DisableTfaSetup([FromBody] TfaSetupDTO tfaSetupDTO)
        {
            var claims = HttpContext.GetUserClaims();
            var userId = JwtService.GetUserGuidFromClaims(claims);
            var password = JwtService.GetUserPasswordFromClaims(claims);

            var result = await _authService.DisableTfa(userId, password, tfaSetupDTO);

            if (result is null)
                return BadRequest();

            return Ok(result);
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
