using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLib
{
    public static class Parser
    {
        public static string ByteArrayToString(byte[] arrInput)
        {
            return Encoding.UTF8.GetString(arrInput);
        }
    }
}
