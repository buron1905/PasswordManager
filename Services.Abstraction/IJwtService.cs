using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IJwtService
    {
        public string GenerateJweToken(string masterPassword, SecurityKey signingKey, SecurityKey encryptionKey);
        public string? ValidateJweToken(string token, SecurityKey signingKey, SecurityKey encryptionKey);
    }
}
