using System.Security.Cryptography;
using System.Text;

namespace Services.Cryptography
{
    public static class EncryptionService
    {
        public static async Task<byte[]> EncryptAsync(string clearText, string passphrase)
        {
            var IV = GenerateRandomIV();
            var salt = GenerateRandomSalt();
            var key = DeriveKeyFromPassword(passphrase, salt);

            return await EncryptStringToBytes_Aes(clearText, key, IV, salt);
        }

        public static async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
        {
            return await DecryptStringFromBytes_Aes(encrypted, passphrase);
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

    }
}
