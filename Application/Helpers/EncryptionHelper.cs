using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Application.Helpers
{
    public class EncryptionHelper
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionHelper(IConfiguration configuration)
        {
            // Get Key and IV from appsettings or secrets
            var keyString = configuration["Encryption:Key"];
            var ivString = configuration["Encryption:IV"];

            if (string.IsNullOrWhiteSpace(keyString) || string.IsNullOrWhiteSpace(ivString))
                throw new InvalidOperationException("Encryption key or IV not configured.");

            _key = Encoding.UTF8.GetBytes(keyString);
            _iv = Encoding.UTF8.GetBytes(ivString);

            if (_key.Length != 32 || _iv.Length != 16)
                throw new InvalidOperationException("Key must be 32 bytes and IV must be 16 bytes (for AES-256).");
        }

        public byte[] Encrypt(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            return PerformCryptography(data, encryptor);
        }

        public byte[] Decrypt(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            return PerformCryptography(data, decryptor);
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return ms.ToArray();
        }
    }
}
