using System.Security.Cryptography;
using System.Text;

namespace Services.Cryptography
{
    public static class HashingService
    {
        // BCrypt should automatically generate salt and store it with the hash
        public static string HashPassword(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
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
    }
}
