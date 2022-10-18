using Microsoft.IdentityModel.Tokens;
using PasswordManager.WebAPI.Helpers;
using Services.Abstraction;
using Services.TMP;
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

        public async Task Invoke(HttpContext context, IJwtService jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var password = jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);
                if (password != null)
                {
                    // attach user to context on successful jwt validation
                    context.Items["password"] = password;
                    //context.Items["User"] = userService.GetById(userId.Value);
                }
            }

            await _next(context);
        }
    }
}
