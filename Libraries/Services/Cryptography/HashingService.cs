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

    }
}
