using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Services.TMP
{
    public static class EncryptionKeys
    {
        // TODO move securely keys
        public static RSA _encryptionKey = RSA.Create(3072); // public key for encryption, private key for decryption
        public static ECDsa _signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256); // private key for signing, public key for validation
        public static string _encryptionKid = Guid.NewGuid().ToString("N");
        public static string _signingKid = Guid.NewGuid().ToString("N");
        public static RsaSecurityKey _privateEncryptionKey = new RsaSecurityKey(_encryptionKey) { KeyId = _encryptionKid };
        public static RsaSecurityKey _publicEncryptionKey = new RsaSecurityKey(_encryptionKey.ExportParameters(false)) { KeyId = _encryptionKid };
        public static ECDsaSecurityKey _privateSigningKey = new ECDsaSecurityKey(_signingKey) { KeyId = _signingKid };
        public static ECDsaSecurityKey _publicSigningKey = new ECDsaSecurityKey(ECDsa.Create(_signingKey.ExportParameters(false))) { KeyId = _signingKid };
    }
}
