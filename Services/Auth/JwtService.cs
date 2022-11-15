using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Abstraction.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Auth
{
    public class JwtService : IJwtService
    {
        public string GenerateJweToken(IEnumerable<Claim> claims, SecurityKey signingKey, SecurityKey encryptionKey, DateTime expires)
        {
            var handler = new JsonWebTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "https://localhost:5001",
                Issuer = "https://localhost:5001",
                Claims = new Dictionary<string, object> { { "sub", "811e790749a24d8a8f766e1a44dca28a" } },
                Subject = new ClaimsIdentity(claims),

                // private key for signing
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.EcdsaSha256),

                // public key for encryption
                EncryptingCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512),

                Expires = DateTime.UtcNow.AddMinutes(10)
            };

            return handler.CreateToken(tokenDescriptor);
        }
        
        public string GenerateJwtToken(IEnumerable<Claim> claims, string secret, DateTime expires)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: expires,
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public IEnumerable<Claim>? ValidateJweToken(string token, SecurityKey signingKey, SecurityKey encryptionKey, bool validateLifetime = true)
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
                    ValidateLifetime = validateLifetime,

                    // public key for signing
                    IssuerSigningKey = signingKey,

                    // private key for encryption
                    TokenDecryptionKey = encryptionKey,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                });

            if (result.IsValid)
                return result.ClaimsIdentity.Claims;
            else
                return null;
        }

        public IEnumerable<Claim> GetClaims(string emailAddress, string password, DateTime expirationUTCDateTime)
        {
            var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, emailAddress),
                    new Claim("password", password),
                    new Claim(ClaimTypes.Expiration, expirationUTCDateTime.ToString())
                };

            //if (expirationDateTime is not null)
            //    claims.Add(new Claim(ClaimTypes.Expiration, expirationDateTime.ToString()));

            return claims;
        }

    }
}
