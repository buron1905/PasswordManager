using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using MAUICryptographyLib;

namespace PasswordManager.Services
{
    public enum HashTypeEnum
    {
        SHA512ToString,
        SHA512ToBytes,
        SHA256ToString,
        SHA256ToBytes,
        MD5ToString,
        MD5ToBytes
    }

    public static class HashingService
    {

        public static T Hash<T>(string text, HashTypeEnum hashType) where T : class
        {
            switch (hashType)
            {
                case HashTypeEnum.SHA512ToString:
                    return (T)(object)HashSHA512ToString(text);
                case HashTypeEnum.SHA256ToString:
                    return (T)(object)HashSHA256ToString(text);
                case HashTypeEnum.MD5ToString:
                    return (T)(object)HashMD5ToString(text);
                case HashTypeEnum.SHA512ToBytes:
                    return (T)(object)HashSHA512ToBytes(text);
                case HashTypeEnum.SHA256ToBytes:
                    return (T)(object)HashSHA256ToBytes(text);
                case HashTypeEnum.MD5ToBytes:
                    return (T)(object)HashMD5ToBytes(text);
                default:
                    return default(T);
            }
        }

        public static string HashSHA512ToString(string text)
        {
            return Hasher.HashSHA512ToString(text);
        }

        public static string HashSHA256ToString(string text)
        {
            return Hasher.HashSHA256ToString(text);
        }

        public static string HashMD5ToString(string text)
        {
            return Hasher.HashMD5ToString(text);
        }

        public static byte[] HashSHA512ToBytes(string text)
        {
            return Hasher.HashSHA512ToBytes(text);
        }

        public static byte[] HashSHA256ToBytes(string text)
        {
            return Hasher.HashSHA256ToBytes(text);
        }

        public static byte[] HashMD5ToBytes(string text)
        {
            return Hasher.HashMD5ToBytes(text);
        }

    }
}
