using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BusinessLogic
{
    public static class HashMethods
    {
        public static string HashMD5(this string source)
        {
            //first convert the input string into a byte array
            byte[] data;

            //create a .net hash provider object
            using (MD5 md5hash = MD5.Create())
            {
                data = md5hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            //create an output stringbuilder
            var s = new StringBuilder();

            //hash the input

            //loop thru the hash creating letters for the stringbuilder
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            //return the hex string representation of the hash
            return s.ToString();

        }

        public static string HashSha1(this string source)
        {
            //first convert the input string into a byte array
            byte[] data;

            //create a .net hash provider object
            using (SHA1 sha1Hash = SHA1.Create())
            {
                data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            //create an output stringbuilder
            var s = new StringBuilder();

            //hash the input

            //loop thru the hash creating letters for the stringbuilder
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            //return the hex string representation of the hash
            return s.ToString();
        }

        public static string HashSha256(this string source)
        {
            //first convert the input string into a byte array
            byte[] data;

            //create a .net hash provider object
            using (SHA256 sha256Hash = SHA256.Create())
            {
                data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            //create an output stringbuilder
            var s = new StringBuilder();

            //hash the input

            //loop thru the hash creating letters for the stringbuilder
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            //return the hex string representation of the hash
            return s.ToString();
        }

        public static string HashSha512(this string source)
        {
            //first convert the input string into a byte array
            byte[] data;

            //create a .net hash provider object
            using (SHA512 sha512Hash = SHA512.Create())
            {
                data = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            //create an output stringbuilder
            var s = new StringBuilder();

            //loop thru the hash creating letters for the stringbuilder
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            //return the hex string representation of the hash
            return s.ToString();
        }

        public static bool VerifyMd5Hash(this string compareString, string source)
        //compareString is hex rep, source is ord string to check
        {
            //use a string compare to compare values
            var c = StringComparer.OrdinalIgnoreCase;

            //do comparison in return
            return (0 == c.Compare(compareString.HashMD5(), source));
        }

        public static bool VerifySha1Hash(this string compareString, string source)
        //compareString is hex rep, source is ord string to check
        {
            //use a string compare to compare values
            var c = StringComparer.OrdinalIgnoreCase;

            //do comparison in return
            return (0 == c.Compare(compareString.HashSha1(), source));
        }

        public static bool VerifySha256Hash(this string compareString, string source)
        //compareString is hex rep, source is ord string to check
        {
            //use a string compare to compare values
            var c = StringComparer.OrdinalIgnoreCase;

            //do comparison in return
            return (0 == c.Compare(compareString.HashSha256(), source));
        }

        public static bool VerifySha512Hash(this string compareString, string source)
        //compareString is hex rep, source is ord string to check
        {
            //use a string compare to compare values
            var c = StringComparer.OrdinalIgnoreCase;

            //do comparison in return
            return (0 == c.Compare(compareString.HashSha512(), source));
        }
    }
}
