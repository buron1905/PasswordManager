using Models;
using Services.Cryptography;

namespace PasswordManager.MAUI.Services
{
    public class RegistrationService
    {
        public static async Task<bool> Register(string email, string password)
        {
            string passwordHASH = HashingService.HashSHA512ToString(password);

            byte[] arr = EncryptionService.EncryptStringToBytes_Aes(email, passwordHASH);
            string emailEncrypted = ParsingService.ByteArrayToString(arr);

            User user = await DatabaseService.GetUser(email);
            if (user != null)
            {
                if (passwordHASH == user.PasswordHASH)
                {
                    ActiveUserService.Instance.Login(user, password);
                    return true;
                }
            }

            return false;
        }
    }
}
