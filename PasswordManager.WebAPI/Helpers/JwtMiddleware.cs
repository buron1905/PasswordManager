using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace PasswordManager.WebAPI.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        
        
        // TODO move securely keys
        public static RSA _encryptionKey = RSA.Create(3072); // public key for encryption, private key for decryption
        public static ECDsa _signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256); // private key for signing, public key for validation
        public static string _encryptionKid = Guid.NewGuid().ToString("N");
        public static string _signingKid = Guid.NewGuid().ToString("N");
        public static RsaSecurityKey _privateEncryptionKey = new RsaSecurityKey(_encryptionKey) { KeyId = _encryptionKid };
        public static RsaSecurityKey _publicEncryptionKey = new RsaSecurityKey(_encryptionKey.ExportParameters(false)) { KeyId = _encryptionKid };
        public static ECDsaSecurityKey _privateSigningKey = new ECDsaSecurityKey(_signingKey) { KeyId = _signingKid };
        public static ECDsaSecurityKey _publicSigningKey = new ECDsaSecurityKey(ECDsa.Create(_signingKey.ExportParameters(false))) { KeyId = _signingKid };

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var password = jwtUtils.ValidateToken(token, _publicSigningKey, _privateEncryptionKey);
            if (password != null)
            {
                // attach user to context on successful jwt validation
                context.Items["password"] = password;
                //context.Items["User"] = userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
