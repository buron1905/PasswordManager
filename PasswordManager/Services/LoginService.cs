using MAUIModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class LoginService
    {
        public static async Task<bool> Login(string email, string password)
        {
            string passwordHASH = HashingService.HashSHA512ToString(password);

            User user = await DatabaseService.GetUser(email);
            if(user != null)
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
