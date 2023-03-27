using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public IEnumerable<Claim> GetClaims(Guid userId, string emailAddress, string password, DateTime expirationUTCDateTime, bool tfaChecked = true, bool emailConfirmed = true)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, emailAddress),
                    new Claim("password", password),
                    new Claim(ClaimTypes.Expiration, expirationUTCDateTime.ToString()),
                    new Claim("tfaChecked", tfaChecked.ToString()),
                    new Claim("emailConfirmed", emailConfirmed.ToString())
                };

            return claims;
        }

        public static Guid GetUserGuidFromClaims(IEnumerable<Claim> claims)
        {
            var userId = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            if (userId is null)
                return Guid.Empty;

            return new Guid(userId);
        }

        public static string? GetUserEmailFromClaims(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        }

        public static string? GetUserPasswordFromClaims(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals("password"))?.Value;
        }

        /// <summary>
        /// Default is true
        /// </summary>
        public static bool GetTfaCheckedFromClaims(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals("tfaChecked"))?.Value != false.ToString(); // default is true
        }

        public static bool GetEmailConfirmedFromClaims(IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals("emailConfirmed"))?.Value == true.ToString();
        }

        public static DateTime? GetTokenExpirationFromClaims(IEnumerable<Claim> claims)
        {
            var dateTimeString = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Expiration))?.Value;

            return DateTime.TryParse(dateTimeString, out DateTime dateTime) ? dateTime : null;
        }
    }
}
