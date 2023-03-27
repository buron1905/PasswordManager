using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Services.Abstraction.Auth
{
    public interface IJwtService
    {
        string GenerateJweToken(IEnumerable<Claim> claims, SecurityKey signingKey, SecurityKey encryptionKey, DateTime expires);
        string GenerateJwtToken(IEnumerable<Claim> claims, string secret, DateTime expires);
        IEnumerable<Claim>? ValidateJweToken(string token, SecurityKey signingKey, SecurityKey encryptionKey, bool validateLifetime = true);
        IEnumerable<Claim> GetClaims(Guid userId, string emailAddress, string password, DateTime expirationDateTime, bool tfaChecked = true, bool emailConfirmed = true);
    }
}
