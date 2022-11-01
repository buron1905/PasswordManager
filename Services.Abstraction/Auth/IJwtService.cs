using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Auth
{
    public interface IJwtService
    {
        public string GenerateJweToken(string masterPassword, SecurityKey signingKey, SecurityKey encryptionKey);
        public string? ValidateJweToken(string token, SecurityKey signingKey, SecurityKey encryptionKey, bool validateLifetime = true);
        public string GenerateJwtToken(string userId, string emailAddress, string secret);
        public RefreshToken GenerateRefreshToken(string token, DateTime expires, DateTime? created = null);
    }
}
