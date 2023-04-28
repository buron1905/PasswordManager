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
        public static async Task<byte[]> EncryptUsingAesAsync(string plainTextData, string plainTextKey)
        {
            var IV = GenerateRandomIV();
            var salt = GenerateRandomSalt();
            var key = DeriveKeyFromPassword(plainTextKey, salt);

            return await EncryptStringToBytes_Aes(plainTextData, key, IV, salt);
        }

        public static async Task<string> DecryptUsingAesAsync(byte[] encryptedData, string plainTextKey)
        {
            return await DecryptStringFromBytes_Aes(encryptedData, plainTextKey);
        }

        public static string? EncryptUsingRsa(string plainTextData, string plainTextKey)
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

        public static string? DecryptUsingRsa(string encryptedData, string plainTextKey)
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
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

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
                aesAlg.Padding = PaddingMode.PKCS7;
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

        public async static Task<string> Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            string plainText = string.Empty;

            using (Aes aes = Aes.Create())
            {
                // extract salt, iv and encryptedText
                var salt = cipherBytes.Take(32).ToArray();
                var iv = cipherBytes.Skip(32).Take(16).ToArray();
                var encryptedBytes = cipherBytes.Skip(48).ToArray();

                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.KeySize = 256;
                aes.Key = DeriveKeyFromPassword(password, salt);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            plainText = await streamReader.ReadToEndAsync();
                        }
                    }
                }
            }

            return plainText;
        }

        public async static Task<string> Encrypt(string plainText, string password)
        {
            var IV = GenerateRandomIV();
            var salt = GenerateRandomSalt();
            var key = DeriveKeyFromPassword(plainText, salt);

            byte[] cipherBytes;

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
                            streamWriter.Write(plainText);
                        }
                    }
                    cipherBytes = memoryStream.ToArray();
                }
            }
            // save IV and salt along with encrypted data
            byte[] cipherBytesWithSaltAndIV = new byte[16 + 32 + cipherBytes.Length];
            Array.Copy(salt, 0, cipherBytesWithSaltAndIV, 0, 32);
            Array.Copy(IV, 0, cipherBytesWithSaltAndIV, 32, 16);
            Array.Copy(cipherBytes, 0, cipherBytesWithSaltAndIV, 48, cipherBytes.Length);
            cipherBytes = cipherBytesWithSaltAndIV;

            return Convert.ToBase64String(cipherBytes);
        }

    }
}

// Encrypt method was inspired by https://www.appsloveworld.com/csharp/100/310/encrypting-in-angular-and-decrypt-on-c
// Encrypt and decrypt methods were inspired by https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-7.0
// RSA based on https://github.com/Technosaviour/RSA-net-core/tree/RSA-Angular-net-core