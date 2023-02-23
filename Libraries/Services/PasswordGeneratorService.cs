using Models.DTOs;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services
{
    public static class PasswordGeneratorService
    {
        private const string _letters = "abcdefghijklmnopqrstuvwxyz";
        private const string _numbers = "0123456789";
        private const string _specialChars = "!@#$%^&*";

        public static string GeneratePassword(PasswordGeneratorSettingsDTO settings)
        {
            return PasswordGeneratorService.GeneratePassword(settings.PasswordLength, settings.UseNumbers, settings.UseSpecialChars, settings.UseUppercase, settings.UseLowercase);
        }

        public static string GeneratePassword(int length = 8, bool useNumbers = true, bool useSpecialChars = true, bool useUppercase = true, bool useLowercase = true)
        {
            if (length < 1)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            if (useNumbers) sb.Append(_numbers);
            if (useSpecialChars) sb.Append(_specialChars);
            if (useUppercase) sb.Append(_letters.ToUpperInvariant());
            if (useLowercase) sb.Append(_letters);

            string alphabet = sb.ToString();
            if (alphabet.Length == 0)
                return string.Empty;

            string password;
            do
            {
                sb.Clear();
                for (int i = 0; i < length; i++)
                {
                    int next = RandomNumberGenerator.GetInt32(alphabet.Length);
                    sb.Append(alphabet[next]);
                }
                password = sb.ToString();
            } while (!CheckConstraints(password, useNumbers, useSpecialChars, useUppercase, useLowercase));


            return password;
        }

        private static bool CheckConstraints(string text, PasswordGeneratorSettingsDTO settings)
        {
            return PasswordGeneratorService.CheckConstraints(text, settings.UseNumbers, settings.UseSpecialChars, settings.UseUppercase, settings.UseLowercase);
        }

        private static bool CheckConstraints(string text, bool useNumbers = true, bool useSpecialChars = true, bool useUppercase = true, bool useLowercase = true)
        {
            if (useNumbers && !text.Any(char.IsDigit))
                return false;
            else if (!useNumbers && text.Any(char.IsDigit))
                return false;

            if (useUppercase && !text.Any(char.IsUpper))
                return false;
            else if (!useUppercase && text.Any(char.IsUpper))
                return false;

            if (useLowercase && !text.Any(char.IsLower))
                return false;
            else if (!useLowercase && text.Any(char.IsLower))
                return false;

            string strRegex = @"[!@#$%^&*]";

            Regex re = new Regex(strRegex);

            if (useSpecialChars)
            {
                if (!re.IsMatch(text))
                    return false;
            }
            else if (!useSpecialChars)
            {
                if (re.IsMatch(text))
                    return false;
            }

            return true;
        }
    }
}
