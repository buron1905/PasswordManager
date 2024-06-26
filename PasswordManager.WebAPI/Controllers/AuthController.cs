﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using Models.DTOs;
using PasswordManager.WebAPI.Extensions;
using PasswordManager.WebAPI.Helpers.Attributes;
using Services.Abstraction.Auth;
using Services.Abstraction.Data;
using Services.Auth;
using Services.Cryptography;
using Services.TMP;

namespace PasswordManager.WebAPI.Controllers
{
    public class AuthController : ApiControllerBase
    {
        #region Private fields

        private readonly AppSettings _appSettings;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        #endregion

        public AuthController(IOptions<AppSettings> appSettings, IUserService userService, IAuthService authService, IJwtService jwtService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
        }

        #region Actions

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            loginDTO.Password = EncryptionService.DecryptUsingRSA(loginDTO.Password, EncryptionKeys.privateRsaKey);

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
            registerDTO.Password = EncryptionService.DecryptUsingRSA(registerDTO.Password, EncryptionKeys.privateRsaKey);
            registerDTO.ConfirmPassword = EncryptionService.DecryptUsingRSA(registerDTO.ConfirmPassword, EncryptionKeys.privateRsaKey);

            if (!registerDTO.Password.Equals(registerDTO.ConfirmPassword))
                return BadRequest(new AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Passwords do not match" });

            var response = await _authService.RegisterAsync(registerDTO);

            if (response == null)
                return Unauthorized(new AuthResponseDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid Authentication" });

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

            if (!await _authService.ResendConfirmEmail(email))
                return BadRequest("Invalid Resend Confirmation Email Request");

            return Ok();
        }

        #region TFA

        [HttpPost("tfa-login")]
        public async Task<IActionResult> TfaLogin([FromBody] LoginWithTfaRequestDTO requestDTO)
        {
            requestDTO.Password = EncryptionService.DecryptUsingRSA(requestDTO.Password, EncryptionKeys.privateRsaKey);

            var response = await _authService.LoginWithTfaAsync(requestDTO);

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

            var result = await _authService.GetTfaSetup(userId);

            if (result is null)
                return BadRequest();

            return Ok(result);
        }

        [Authorize]
        [HttpPost("tfa-setup")]
        public async Task<IActionResult> TfaSetup([FromBody] TfaSetupDTO tfaSetupDTO)
        {
            var claims = HttpContext.GetUserClaims();
            var userId = JwtService.GetUserGuidFromClaims(claims);
            var email = JwtService.GetUserEmailFromClaims(claims);

            TfaSetupDTO? result = null!;

            if (tfaSetupDTO.IsTfaEnabled)
                result = await _authService.EnableTfa(userId, tfaSetupDTO);
            else
                result = await _authService.DisableTfa(userId, tfaSetupDTO);

            if (result is null)
                return BadRequest();

            var user = await _userService.GetByIdAsync(userId);

            var response = _authService.GetAuthResponse(user, email, tfaEnabled: user.TwoFactorEnabled, emailVerified: user.EmailConfirmed);
            SetTokenCookie(response.JweToken!);

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
