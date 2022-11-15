﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using PasswordManager.WebAPI.Helpers;
using Services.Abstraction.Auth;
using Services.Auth;
using Services.TMP;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PasswordManager.WebAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService, IOptions<AppSettings> appSettings)
        {
            //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var token = context.Request.Cookies["token"] ?? context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var claims = jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);
                if (claims != null)
                {
                    // attach user to context on successful jwt validation
                    context.Items["emailAddress"] = claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value;

                    if (context.Request.Cookies["token"] is not null)
                    {
                        var expires = DateTime.UtcNow.AddMinutes(appSettings.Value.JweTokenMinutesTTL);
                        var newClaims = jwtService.GetClaims(claims.First(c => c.Type.Equals(ClaimTypes.Email)).Value, claims.First(c => c.Type.Equals("password")).Value, expires);
                        var newToken = jwtService.GenerateJweToken(newClaims, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey, expires);
                    
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = expires
                        };
                        context.Response.Cookies.Append("token", newToken, cookieOptions);
                    }
                }
            }

            await _next(context);
        }
    }
}
