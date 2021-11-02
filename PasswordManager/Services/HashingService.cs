using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace PasswordManager.Services
{
    public static class HashingService
    {
        public static string Hash(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash;
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(data);
            }

            return ByteArrayToString(hash);
        }

        static string ByteArrayToString(byte[] arrInput)
        {
            return Encoding.UTF8.GetString(arrInput);

            //int i;
            //StringBuilder sOutput = new StringBuilder(arrInput.Length);
            //for (i = 0; i < arrInput.Length - 1; i++)
            //{
            //    sOutput.Append(arrInput[i].ToString("X2"));
            //}
            //return sOutput.ToString();
        }
    }
}



//using System;
//using System.Security.Cryptography;
//using System.Text;

//namespace ComputeAHash_csharp
//{
//    /// <summary>
//    /// Summary description for Class1.
//    /// </summary>
//    class Class1
//    {
//        static void Main(string[] args)
//        {
//            string sSourceData;
//            byte[] tmpSource;
//            byte[] tmpHash;
//            sSourceData = "MySourceData";
//            //Create a byte array from source data
//            tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

//            //Compute hash based on source data
//            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
//            Console.WriteLine(ByteArrayToString(tmpHash));

//            sSourceData = "NotMySourceData";
//            tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

//            byte[] tmpNewHash;

//            tmpNewHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

//            bool bEqual = false;
//            if (tmpNewHash.Length == tmpHash.Length)
//            {
//                int i = 0;
//                while ((i < tmpNewHash.Length) && (tmpNewHash[i] == tmpHash[i]))
//                {
//                    i += 1;
//                }
//                if (i == tmpNewHash.Length)
//                {
//                    bEqual = true;
//                }
//            }

//            if (bEqual)
//                Console.WriteLine("The two hash values are the same");
//            else
//                Console.WriteLine("The two hash values are not the same");
//            Console.ReadLine();
//        }

//        static string ByteArrayToString(byte[] arrInput)
//        {
//            int i;
//            StringBuilder sOutput = new StringBuilder(arrInput.Length);
//            for (i = 0; i < arrInput.Length - 1; i++)
//            {
//                sOutput.Append(arrInput[i].ToString("X2"));
//            }
//            return sOutput.ToString();
//        }
//    }
//}