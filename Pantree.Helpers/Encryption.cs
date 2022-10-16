using System.Security.Cryptography;
using System.Text;

namespace Pantree.Helpers
{
    /// <summary>
    /// Simple class to encrypt and decrypt strings.
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// Encrypts a string using Advanced Encryption Standard.
        /// </summary>
        /// <param name="input">The data to be encrypted.</param>
        /// <param name="key">The encryption key used to encrypt the data.</param>
        /// <returns>The encrypted data, or null if encryption failed.</returns>
        public static string Encrypt(string input, string key)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(input);

                using var encryptor = Aes.Create();
                var pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(clearBytes, 0, clearBytes.Length);
                        cryptoStream.Close();
                    }
                    input = Convert.ToBase64String(memoryStream.ToArray());
                }
                return input;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts a string using Advanced Encryption Standard.
        /// </summary>
        /// <param name="input">The data to be decrypted.</param>
        /// <param name="key">The encryption key used when encrypting the data, to decrypt it.</param>
        /// <returns>The decrypted data, or null if decryption failed.</returns>
        public static string Decrypt(string input, string key)
        {
            try
            {
                input = input.Replace(" ", "+");
                var cipherBytes = Convert.FromBase64String(input);

                using (var encryptor = Aes.Create())
                {
                    var pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using var memoryStream = new MemoryStream();
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                        cryptoStream.Close();
                    }

                    input = Encoding.Unicode.GetString(memoryStream.ToArray());
                }
                return input;
            }
            catch
            {
                return null;
            }
        }
    }
}