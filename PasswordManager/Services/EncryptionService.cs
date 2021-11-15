using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using CryptographyLib;

namespace PasswordManager.Services
{
    public static class EncryptionService
    {

        public static byte[] EncryptStringToBytes_Aes(string plainText, string key)
        {
            return Encrypter.EncryptStringToBytes_Aes(plainText, HashingService.HashSHA256ToBytes(key), HashingService.HashMD5ToBytes(key));
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, string key)
        {
            return Encrypter.DecryptStringFromBytes_Aes(cipherText, HashingService.HashSHA256ToBytes(key), HashingService.HashMD5ToBytes(key));
        }

    }
}