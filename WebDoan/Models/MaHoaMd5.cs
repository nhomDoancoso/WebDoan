using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebDoan.Models
{
    public class MaHoaMd5
    {
        public static string GetHashMD5(string plainText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            // Compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText));
            // Get hash result after compute it
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}