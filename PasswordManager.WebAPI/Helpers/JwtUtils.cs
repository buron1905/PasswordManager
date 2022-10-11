using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.WebAPI.Helpers
{
    public class JwtUtils : IJwtUtils
    {


        public string GenerateToken(string masterPassword, SecurityKey signingKey, SecurityKey encryptionKey)
        {
            var handler = new JsonWebTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "https://localhost:5001",
                Issuer = "https://localhost:5001",
                Claims = new Dictionary<string, object> { { "sub", "811e790749a24d8a8f766e1a44dca28a" } },
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("password", masterPassword)
                }),

                // private key for signing
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.EcdsaSha256),

                // public key for encryption
                EncryptingCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512),
                
                Expires = DateTime.UtcNow.AddMinutes(10)
            };

            return handler.CreateToken(tokenDescriptor);
        }

        public string? ValidateToken(string token, SecurityKey signingKey, SecurityKey encryptionKey)
        {
            var handler = new JsonWebTokenHandler();

            TokenValidationResult result = handler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "https://localhost:5001",
                    ValidIssuer = "https://localhost:5001",
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,

                    // public key for signing
                    IssuerSigningKey = signingKey,

                    // private key for encryption
                    TokenDecryptionKey = encryptionKey,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                });

            if (result.IsValid)
                return result.ClaimsIdentity.Claims.First(x => x.Type == "password").Value.ToString();
                //return result.Claims.First(x => x.Key == "password").Value.ToString();
            else
                return null;
        }
    }
}
