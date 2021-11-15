using MAUICryptographyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public static class ParserService
    {
        public static string ByteArrayToString(byte[] arrInput)
        {
            return Parser.ByteArrayToString(arrInput);
        }
    }
}
