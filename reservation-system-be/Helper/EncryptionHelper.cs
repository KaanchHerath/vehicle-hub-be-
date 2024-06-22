using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace reservation_system_be.Helper
{
    public static class EncryptionHelper
    {
        private static readonly string EncryptionKey = "vehicle-hub"; // Use a secure key

        public static string Encrypt(int Id)
        {
            string clearText = Id.ToString();
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] salt = new byte[16];
                RandomNumberGenerator.Fill(salt); // Use RandomNumberGenerator instead of RNGCryptoServiceProvider

                // Use SHA256 and 100000 iterations for key derivation
                var pdb = new Rfc2898DeriveBytes(EncryptionKey, salt, 100000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length); // Write the salt to the beginning of the output
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    byte[] encryptedBytes = ms.ToArray();
                    string base64 = Convert.ToBase64String(encryptedBytes);

                    base64 = base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
                    clearText = base64;
                }
            }
            return clearText;
        }

        public static int Decrypt(string cipherText)
        {
            // Adjust the Base64 URL-safe format
            cipherText = cipherText.Replace("-", "+").Replace('_', '/');
            int mod4 = cipherText.Length % 4;
            if (mod4 > 0)
            {
                cipherText += new string('=', 4 - mod4);
            }

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (var ms = new MemoryStream(cipherBytes))
            {
                byte[] salt = new byte[16];
                ms.Read(salt, 0, salt.Length); // Read the salt from the beginning of the input

                using (Aes encryptor = Aes.Create())
                {
                    // Use SHA256 and 100000 iterations for key derivation
                    var pdb = new Rfc2898DeriveBytes(EncryptionKey, salt, 100000, HashAlgorithmName.SHA256);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] clearBytes = new byte[cipherBytes.Length - salt.Length];
                        int readBytes = cs.Read(clearBytes, 0, clearBytes.Length);
                        string clearText = Encoding.Unicode.GetString(clearBytes, 0, readBytes);
                        return int.Parse(clearText);
                    }
                }
            }
        }
    }
}
