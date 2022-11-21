using System.Text;

namespace Services.Cryptography
{
    public static class ParsingService
    {
        public static string ByteArrayToString(byte[] arrInput)
        {
            return Encoding.UTF8.GetString(arrInput);
        }
    }
}
