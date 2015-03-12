using System.IO;
using System.Security.Cryptography;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    class SymmetricEncryptor : ISymmetricEncryptor
    {
        private static readonly byte[] Salt = new byte[]
                                                  {
                                                      0xa1,
                                                      0x14,
                                                      0x55,
                                                      0x21,
                                                      0xbc,
                                                      0x75,
                                                      0xf1,
                                                      0xfe,
                                                      0xbc,
                                                      0x32
                                                  };

        private const int Iterations = 1000;
        private const int KeySize = 32;

        private static SymmetricKey GetAESKeyFromPassword(string password)
        {
            byte[] keyBytes = new Rfc2898DeriveBytes(password, Salt, Iterations).GetBytes(KeySize);
            return new SymmetricKey(keyBytes);
        }

        private byte[] EncryptOrDecrypt(byte[] bytes, SymmetricKey key, InitializationVector iv, bool decrypt)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider { Key = key.Value, IV = iv.Value })
            {
                using (ICryptoTransform cryptoTransform = decrypt ? aes.CreateDecryptor() : aes.CreateEncryptor())
                {
                    return cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                }
            }
        }

        private void EncryptOrDecrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv, bool decrypt)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider { Key = key.Value, IV = iv.Value })
            {
                using (ICryptoTransform cryptoTransform = decrypt ? aes.CreateDecryptor() : aes.CreateEncryptor())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(inputStream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        BlockCopy(cryptoStream, outputStream);
                        outputStream.Flush();
                        outputStream.Close();
                    }
                }
            }
        }

        private void BlockCopy(Stream input, Stream output)
        {
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
                output.Flush();
            }
        }

        public byte[] Encrypt(byte[] bytes, SymmetricKey key, InitializationVector iv)
        {
            return EncryptOrDecrypt(bytes, key, iv, false);
        }

        public byte[] Decrypt(byte[] bytes, SymmetricKey key, InitializationVector iv)
        {
            return EncryptOrDecrypt(bytes, key, iv, true);
        }

        public EncryptedPrivateKey EncryptWithPassword(string password, InitializationVector iv, PrivateKey data)
        {
            return new EncryptedPrivateKey(EncryptOrDecrypt(data.Value, GetAESKeyFromPassword(password), iv, false));
        }

        public PrivateKey DecryptWithPassword(string password, InitializationVector iv, EncryptedPrivateKey data)
        {
            return new PrivateKey(EncryptOrDecrypt(data.Value, GetAESKeyFromPassword(password), iv, true));
        }

        public void Encrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv)
        {
            EncryptOrDecrypt(inputStream, outputStream, key, iv, false);
        }

        public void Decrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv)
        {
            EncryptOrDecrypt(inputStream, outputStream, key, iv, true);
        }
    }
}
