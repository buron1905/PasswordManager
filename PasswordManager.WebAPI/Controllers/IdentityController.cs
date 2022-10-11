﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using PasswordManager.WebAPI.Services;
using Services.Abstraction;
using Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using PasswordManager.WebAPI.Helpers;

namespace PasswordManager.WebAPI.Controllers
{
    public class IdentityController : ApiControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly AppSettings _appSettings;
        private readonly IIdentityService _identityService;
        

        public IdentityController(ILoggerManager loggerManager, IOptions<AppSettings> appSettings, IIdentityService identityService)
        {
            _logger = loggerManager;
            _appSettings = appSettings.Value;
            _identityService = identityService;

            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.EmailAddress == "admin@admin" && user.Password == "admin")
            {
                _logger.LogInfo("Successful login.");

                var tokenString = new JwtUtils().GenerateToken(user.Password, JwtMiddleware._privateSigningKey, JwtMiddleware._publicEncryptionKey);
                //var tokenString = _identityService.GenerateJwtToken("1", user.EmailAddress, _appSettings.Secret);

                return Ok(new LoginResponseDTO { Token = tokenString });
            }

            return Unauthorized();
        }
    }
}
