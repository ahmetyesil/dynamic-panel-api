using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{

    public class HashManager : ManagerBase<HashManager>
    {

        public string Md5(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            md5.Clear();

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        public string Sha256(string text)
        {
            SHA256Managed sha256 = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = sha256.ComputeHash(Encoding.ASCII.GetBytes(text), 0, Encoding.ASCII.GetByteCount(text));
            sha256.Clear();
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public long HexToInt64(string hexNumber)
        {
            hexNumber = hexNumber.Replace("x", string.Empty);
            long result;
            long.TryParse(hexNumber, NumberStyles.HexNumber, null, out result);
            return result;
        }
    }
}
