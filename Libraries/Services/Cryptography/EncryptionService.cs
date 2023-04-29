using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;

namespace Services.Cryptography
{
    public static class EncryptionService
    {
        #region AES

        public async static Task<string?> EncryptUsingAES(string plainText, string plainTextKey)
        {
            if (string.IsNullOrWhiteSpace(plainText) || string.IsNullOrWhiteSpace(plainTextKey))
                return null;

            var IV = GenerateRandomIV();
            var salt = GenerateRandomSalt();
            var key = DeriveKeyFromPassword(plainTextKey, salt);

            byte[] cipherBytes;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    aes.KeySize = 256;
                    aes.Key = key;
                    aes.IV = IV;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                await streamWriter.WriteAsync(plainText);
                            }
                        }
                        cipherBytes = memoryStream.ToArray();
                    }
                }
                //save IV and salt along with encrypted data
                byte[] cipherBytesWithSaltAndIV = new byte[32 + 16 + cipherBytes.Length];
                Array.Copy(salt, 0, cipherBytesWithSaltAndIV, 0, 32);
                Array.Copy(IV, 0, cipherBytesWithSaltAndIV, 32, 16);
                Array.Copy(cipherBytes, 0, cipherBytesWithSaltAndIV, 48, cipherBytes.Length);
                cipherBytes = cipherBytesWithSaltAndIV;
            }
            catch
            {
                return null;
            }

            return Convert.ToBase64String(cipherBytes);
        }

        public async static Task<string?> DecryptUsingAES(string cipherText, string plainTextKey)
        {
            if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(plainTextKey))
                return null;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            string plainText = null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    // extract salt, iv and encryptedText
                    var salt = cipherBytes.Take(32).ToArray();
                    var iv = cipherBytes.Skip(32).Take(16).ToArray();
                    var encryptedBytes = cipherBytes.Skip(48).ToArray();

                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    aes.KeySize = 256;
                    aes.Key = DeriveKeyFromPassword(plainTextKey, salt);
                    aes.IV = iv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                plainText = await streamReader.ReadToEndAsync();
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return plainText;
        }

        #endregion

        #region RSA

        public static string? EncryptUsingRSA(string plainText, string plainTextKey)
        {
            if (string.IsNullOrWhiteSpace(plainText) || string.IsNullOrWhiteSpace(plainTextKey))
                return null;

            try
            {
                var rsaProvider = GetProviderWithRsaPublicKey(plainTextKey);
                var result = rsaProvider.Encrypt(Encoding.UTF8.GetBytes(plainText), true);

                return Convert.ToBase64String(result);
            }
            catch
            {
                return null;
            }
        }

        public static string? DecryptUsingRSA(string cipherText, string plainTextKey)
        {
            if (string.IsNullOrWhiteSpace(cipherText) || string.IsNullOrWhiteSpace(plainTextKey))
                return null;

            try
            {
                var rsaProvider = GetProviderWithRsaPrivateKey(plainTextKey);
                var result = rsaProvider.Decrypt(Convert.FromBase64String(cipherText), true);

                return Encoding.UTF8.GetString(result);
            }
            catch
            {
                return null;
            }
        }

        #endregion RSA

        #region AED - private methods

        static byte[] GenerateRandomIV()
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }
        }

        static byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[32];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        static byte[] DeriveKeyFromPassword(string password, byte[] salt)
        {
            var iterations = 10000;
            var desiredKeyLength = 32; // in bytes => 256 bits
            var hashAlgorithm = HashAlgorithmName.SHA1;

            // SHA-1 is used because of compatibility with encryption.service.ts in Angular app
            // It is not used for hashing passwords and storing them, just for encryption
            return Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                hashAlgorithm,
                desiredKeyLength);
        }

        #endregion

        #region RSA - private methods

        static RSACryptoServiceProvider GetProviderWithRsaPrivateKey(string pemString)
        {
            using (TextReader privateKeyTextReader = new StringReader(pemString))
            {
                AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

        static RSACryptoServiceProvider GetProviderWithRsaPublicKey(string pemString)
        {
            using (TextReader publicKeyTextReader = new StringReader(pemString))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParam);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

        #endregion
    }
}

// AES encrypt method was inspired by https://www.appsloveworld.com/csharp/100/310/encrypting-in-angular-and-decrypt-on-c
// AES encrypt and decrypt methods were inspired by https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-7.0
// RSA based on https://github.com/Technosaviour/RSA-net-core/tree/RSA-Angular-net-core