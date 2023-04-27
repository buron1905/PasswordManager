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
        public static async Task<byte[]> EncryptAesAsync(string plainTextData, string plainTextKey)
        {
            var IV = GenerateRandomIV();
            var salt = GenerateRandomSalt();
            var key = DeriveKeyFromPassword(plainTextKey, salt);

            return await EncryptStringToBytes_Aes(plainTextData, key, IV, salt);
        }

        public static async Task<string> DecryptAesAsync(byte[] encryptedData, string plainTextKey)
        {
            return await DecryptStringFromBytes_Aes(encryptedData, plainTextKey);
        }

        public static string? EncryptRsa(string plainTextData, string plainTextKey)
        {
            try
            {
                if (plainTextData == null || plainTextData.Length <= 0)
                    throw new ArgumentNullException("plainTextData");
                if (string.IsNullOrEmpty(plainTextKey))
                    throw new ArgumentNullException("plainTextKey");

                var rsaProvider = GetProviderWithRsaPublicKey(plainTextKey);
                var result = rsaProvider.Encrypt(Encoding.UTF8.GetBytes(plainTextData), true);

                return Convert.ToBase64String(result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string? DecryptRsa(string encryptedData, string plainTextKey)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedData))
                    throw new ArgumentNullException("encrypted64BaseData");
                if (string.IsNullOrEmpty(plainTextKey))
                    throw new ArgumentNullException("plainTextKey");

                var rsaProvider = GetProviderWithRsaPrivateKey(plainTextKey);
                var result = rsaProvider.Decrypt(Convert.FromBase64String(encryptedData), true);

                return Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Implementation from https://github.com/Technosaviour/RSA-net-core/tree/RSA-Angular-net-core
        private static RSACryptoServiceProvider GetProviderWithRsaPrivateKey(string pemString)
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

        private static RSACryptoServiceProvider GetProviderWithRsaPublicKey(string pemString)
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
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        static byte[] DeriveKeyFromPassword(string password, byte[] salt)
        {
            var iterations = 10000;
            var desiredKeyLength = 32; // 256 bits
            var hashMethod = HashAlgorithmName.SHA512;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                salt,
                iterations,
                hashMethod,
                desiredKeyLength);
        }

        // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-7.0
        // added async/await, explicit aes and IV alongsite encrypted data
        static async Task<byte[]> EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV, byte[] salt)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7; // ISO10126 is probably better
                aesAlg.KeySize = 256;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    encrypted = msEncrypt.ToArray();
                }

                // save IV along with encrypted data
                byte[] ivAndEncryptedData = new byte[16 + 32 + encrypted.Length];
                Array.Copy(aesAlg.IV, 0, ivAndEncryptedData, 0, 16);
                Array.Copy(salt, 0, ivAndEncryptedData, 16, 32);
                Array.Copy(encrypted, 0, ivAndEncryptedData, 48, encrypted.Length);
                encrypted = ivAndEncryptedData;
            }

            return encrypted;
        }

        static async Task<string> DecryptStringFromBytes_Aes(byte[] cipherText, string passphrase)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7; // ISO10126 is probably better
                aesAlg.KeySize = 256;
                aesAlg.Mode = CipherMode.CBC;

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    byte[] storedIV = new byte[16];
                    msDecrypt.Read(storedIV, 0, 16);
                    aesAlg.IV = storedIV;

                    byte[] storedSalt = new byte[32];
                    msDecrypt.Read(storedSalt, 0, 32);
                    var key = DeriveKeyFromPassword(passphrase, storedSalt);
                    aesAlg.Key = key;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = await srDecrypt.ReadToEndAsync();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static byte[]? RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);

                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

    }
}
