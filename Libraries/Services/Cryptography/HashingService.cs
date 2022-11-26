using System.Security.Cryptography;
using System.Text;

namespace Services.Cryptography
{
    public static class HashingService
    {
        public static string HashSHA512ToString(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;

            using (SHA512 sha = SHA512.Create())
            {
                hash = sha.ComputeHash(data);
            }

            return ParsingService.ByteArrayToString(hash);
        }

        public static string HashSHA256ToString(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;
            using (SHA256 sha = SHA256.Create())
            {
                hash = sha.ComputeHash(data);
            }

            return ParsingService.ByteArrayToString(hash);
        }

        public static string HashMD5ToString(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }

            return ParsingService.ByteArrayToString(hash);
        }


        public static byte[] HashSHA512ToBytes(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;

            using (SHA512 sha = SHA512.Create())
            {
                hash = sha.ComputeHash(data);
            }

            return hash;
        }

        public static byte[] HashSHA256ToBytes(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;
            using (SHA256 sha = SHA256.Create())
            {
                hash = sha.ComputeHash(data);
            }

            return hash;
        }

        public static byte[] HashMD5ToBytes(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(data);
            }

            return hash;
        }
    }
}
