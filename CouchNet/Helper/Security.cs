using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet.Helper
{
    public static class Security
    {
        public static string ComputeHash(string plainText, ref string salt)
        {
            byte[] saltBytes = new byte[0];
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            salt = RandomString(25);
            saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] plainTextWithSaltBytes =                    new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA1Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            string hashValue = ByteArrayToString(hashBytes);
            return hashValue;
        }

        public static string RandomString(int size)
        {
            Random rand = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public static string ByteArrayToString(byte[] buffer)
        {
            StringBuilder hex = new StringBuilder(buffer.Length * 2);
            foreach (byte b in buffer)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
