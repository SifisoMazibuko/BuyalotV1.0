using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuyalotV1._0.Provider
{
    public class Cipher
    {
        static readonly string PasswordHash = "@23sd9e$n";
        static readonly string SaltKey = "7fls9jff7";
        static readonly string VIKey = "^d3%4bxg#8RdU7j(";
        static readonly string NumbericEncryptionKey = "GTHYS^%&hJ5#j^D";

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return plainText;
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

                byte[] cipherTextBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
                return ByteArrayToString(cipherTextBytes);// Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return encryptedText;
            try
            {
                string result = null;
                if (encryptedText != null)
                    encryptedText = encryptedText.Trim();
                byte[] cipherTextBytes = StringToByteArray(encryptedText);// Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        memoryStream.Close();
                        cryptoStream.Close();

                        string tmp = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                        result = tmp;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "").Trim();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string EncodeLong(long? p)
        {
            if (p == null)
                p = 0;
            string plainText = p.Value.ToString() + "_" + NumbericEncryptionKey;
            return Encrypt(plainText);
        }

        public static long DecryptLong(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return 0;

            string v = Decrypt(value);

            var spl = v.Split("_".ToCharArray());
            return Convert.ToInt64(spl[0]);
        }

        public static long StringToLong(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return 0;

            string v = (value);

            var spl = v.Split("_".ToCharArray());
            return Convert.ToInt64(spl[0]);
        }
    }
}