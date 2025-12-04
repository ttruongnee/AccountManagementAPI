using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AccountManagementAPI.Utils
{
    public class EncryptHelper
    {
        private readonly string _key;

        public EncryptHelper(IConfiguration config)
        {
            _key = config["Encryption:Key"];

            if (string.IsNullOrWhiteSpace(_key) || _key.Length != 32)
                throw new Exception("AES Key phải có 32 ký tự đối với AES-256.");
        }


        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.GenerateIV(); // IV sẽ khác mỗi lần
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // lưu IV đầu file
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            byte[] bytes = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                byte[] iv = new byte[16];
                Array.Copy(bytes, 0, iv, 0, 16); // lấy IV từ đầu file
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(bytes, 16, bytes.Length - 16))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
