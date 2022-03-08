using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class PasswordGeneratorService
    {
        public static string GeneratePassword(int length = 8, bool useNumbers = true, bool useSpecialCharacters = true, bool useUppercase = true, bool useLowercase = true)
        {
            if (length < 1)
                return string.Empty;
            
            Random rnd = new Random();

            string password = "Password*1";
            

            return password;
        }
    }
}
