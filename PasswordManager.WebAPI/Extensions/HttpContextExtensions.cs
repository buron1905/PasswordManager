using System.Security.Claims;

namespace PasswordManager.WebAPI.Extensions
{
    public static class HttpContextExtensions
    {
        public static void SetUser(this HttpContext httpContext, IEnumerable<Claim> claims)
        {
            httpContext.SetUser(new ClaimsPrincipal(new ClaimsIdentity(claims)));
        }

        public static void SetUser(this HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
        {
            httpContext.User = claimsPrincipal;
        }

        public static IEnumerable<Claim> GetUserClaims(this HttpContext httpContext)
        {
            return httpContext.User.Claims;
        }
    }
}
