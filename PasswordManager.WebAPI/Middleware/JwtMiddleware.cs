using Microsoft.Extensions.Options;
using Models;
using PasswordManager.WebAPI.Extensions;
using Services.Abstraction.Auth;
using Services.Auth;
using Services.TMP;

namespace PasswordManager.WebAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IJwtService jwtService, IOptions<AppSettings> appSettings)
        {
            var token = httpContext.Request.Cookies["token"] ?? httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token is not null)
            {
                var claims = jwtService.ValidateJweToken(token, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey);

                if (claims is not null && JwtService.GetTfaCheckedFromClaims(claims))
                {
                    httpContext.SetUser(claims);

                    var userId = JwtService.GetUserGuidFromClaims(claims);
                    var emailAddress = JwtService.GetUserEmailFromClaims(claims);
                    var emailConfirmed = JwtService.GetEmailConfirmedFromClaims(claims);
                    var tfaChecked = JwtService.GetTfaCheckedFromClaims(claims);

                    if (userId == Guid.Empty || emailAddress is null)
                        throw new Exception("Some of necessary claims were empty.");

                    // set Cookies
                    if (httpContext.Request.Cookies["token"] is not null)
                    {
                        var expires = DateTime.UtcNow.AddMinutes(appSettings.Value.JweTokenMinutesTTL);
                        var newClaims = jwtService.GetClaims(userId, emailAddress, expires, tfaChecked, emailConfirmed);
                        var newToken = jwtService.GenerateJweToken(newClaims, JWTKeys._publicSigningKey, JWTKeys._privateEncryptionKey, expires);

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = expires
                        };
                        httpContext.Response.Cookies.Append("token", newToken, cookieOptions);
                    }
                }
            }

            await _next(httpContext);
        }
    }
}
