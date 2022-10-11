using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Xunit;
using FluentAssertions;
using static System.Net.WebRequestMethods;

namespace PasswordManager.WebAPI.Helpers
{
    public class JweHelper
    {
        // Code from https://www.scottbrady91.com/c-sharp/json-web-encryption-jwe-in-dotnet-core
        
        [Fact]
        public void CreateAndValidateJwe()
        {
            var encryptionKey = RSA.Create(3072); // public key for encryption, private key for decryption
            var signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256); // private key for signing, public key for validation

            var encryptionKid = Guid.NewGuid().ToString("N");
            var signingKid = Guid.NewGuid().ToString("N");

            var privateEncryptionKey = new RsaSecurityKey(encryptionKey) { KeyId = encryptionKid };
            var publicEncryptionKey = new RsaSecurityKey(encryptionKey.ExportParameters(false)) { KeyId = encryptionKid };
            var privateSigningKey = new ECDsaSecurityKey(signingKey) { KeyId = signingKid };
            var publicSigningKey = new ECDsaSecurityKey(ECDsa.Create(signingKey.ExportParameters(false))) { KeyId = signingKid };

            var token = CreateJwe(privateSigningKey, publicEncryptionKey);
            DecryptAndValidateJwe(token, publicSigningKey, privateEncryptionKey).Should().BeTrue();
        }

        private static string CreateJwe(SecurityKey signingKey, SecurityKey encryptionKey)
        {
            var handler = new JsonWebTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "api",
                //Audience = "https://localhost:5001", 
                Issuer = "https://localhost:5001",
                Claims = new Dictionary<string, object> { { "sub", "811e790749a24d8a8f766e1a44dca28a" } },

                // private key for signing
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.EcdsaSha256),

                // public key for encryption
                EncryptingCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512)
            };

            return handler.CreateToken(tokenDescriptor);
        }

        private static bool DecryptAndValidateJwe(string token, SecurityKey signingKey, SecurityKey encryptionKey)
        {
            var handler = new JsonWebTokenHandler();

            TokenValidationResult result = handler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "api",
                    //Audience = "https://localhost:5001", 
                    ValidIssuer = "https://localhost:5001",

                    // public key for signing
                    IssuerSigningKey = signingKey,

                    // private key for encryption
                    TokenDecryptionKey = encryptionKey
                });

            return result.IsValid;
            //return ClaimsIdentity
            //return Claims
        }
    }
}
