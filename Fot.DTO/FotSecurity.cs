using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fot.DTO
{
   
    public class FotSecurity<T> where T : class
    {
        public static string keyMain = "heoyjehuijwbwjeilhbe892982";
        public static string IVMain = "58779876";


        private static SymmetricAlgorithm createCryptoServiceProvider(string key, string IV)
        {
            byte[] password;

            using (MD5 md5 = MD5.Create())
                password = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            var crypt = new TripleDESCryptoServiceProvider();
            byte[] iv = Encoding.UTF8.GetBytes(IV);
            crypt.IV = iv;
            crypt.Key = password;
            return crypt;
        }


        public static byte[] Serialize(T obj)
        {
            var provider = createCryptoServiceProvider(keyMain, IVMain);
            using (MemoryStream memory = new MemoryStream())
            {
                using (
                    CryptoStream stream = new CryptoStream(memory, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                }
                return memory.ToArray();
            }
        }


        public static T Deserialize(byte[] inBytes)
        {
            var provider = createCryptoServiceProvider(keyMain, IVMain);

            using (MemoryStream memory = new MemoryStream(inBytes))
            {
                using (CryptoStream stream = new CryptoStream(memory, provider.CreateDecryptor(), CryptoStreamMode.Read)
                    )
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (T) formatter.Deserialize(stream);
                }
            }
        }

        public static string Hash(string text)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(text+keyMain);
            var cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }


        
    }
}