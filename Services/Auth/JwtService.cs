using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Auth
{
    public class JwtService : IJwtService
    {
        public string GenerateJweToken(string masterPassword, SecurityKey signingKey, SecurityKey encryptionKey)
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

        public string? ValidateJweToken(string token, SecurityKey signingKey, SecurityKey encryptionKey)
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

        public string GenerateJwtToken(string userId, string emailAddress, string secret)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, emailAddress),
                };

            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
