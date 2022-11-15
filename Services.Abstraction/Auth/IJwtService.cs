using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Auth
{
    public interface IJwtService
    {
        string GenerateJweToken(IEnumerable<Claim> claims, SecurityKey signingKey, SecurityKey encryptionKey, DateTime expires);
        string GenerateJwtToken(IEnumerable<Claim> claims, string secret, DateTime expires);
        IEnumerable<Claim>? ValidateJweToken(string token, SecurityKey signingKey, SecurityKey encryptionKey, bool validateLifetime = true);
        IEnumerable<Claim> GetClaims(string emailAddress, string password, DateTime expirationDateTime);
    }
}
