using Microsoft.IdentityModel.Tokens;
using Models;

namespace PasswordManager.WebAPI.Helpers
{
    public interface IJwtUtils
    {
        public string GenerateToken(string masterPassword, SecurityKey signingKey, SecurityKey encryptionKey);
        public string? ValidateToken(string token, SecurityKey signingKey, SecurityKey encryptionKey);
    }
}
