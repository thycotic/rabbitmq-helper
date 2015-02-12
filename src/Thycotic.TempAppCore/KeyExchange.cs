using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Thycotic.TempAppCore
{

    public interface IKey
    {
        byte[] Value { get; }
    }

    public abstract class KeyBase : IKey
    {
        public byte[] Value
        {
            get;
            internal set;
        }

        protected KeyBase(byte[] key)
        {
            Value = key;
        }

        protected KeyBase()
        {
        }
    }

    public class SymmetricKey : KeyBase
    {
        public SymmetricKey(byte[] symmetricKey)
            : base(symmetricKey)
        {
        }
    }

    public class InitializationVector : KeyBase
    {
        public InitializationVector(byte[] initializationVector)
            : base(initializationVector)
        {
        }
    }
    public class EncryptedSymmetricKey : KeyBase
    {
        public EncryptedSymmetricKey(byte[] encryptedKey)
            : base(encryptedKey)
        {
        }
    }

    public class PublicKey : KeyBase
    {
        public PublicKey(byte[] keyBase)
            : base(keyBase)
        {
        }
    }

    public class PrivateKey : KeyBase
    {
        public PrivateKey(byte[] privateKey)
            : base(privateKey)
        {
        }
    }

    public class EncryptedPrivateKey : KeyBase
    {
        public EncryptedPrivateKey(byte[] encryptedPrivateKey)
            : base(encryptedPrivateKey)
        {
        }
    }

    public interface IAsymmetricEncryptor
    {
        EncryptedSymmetricKey EncryptSymmetricKey(PublicKey publicKey, SymmetricKey key);
        SymmetricKey DecryptSymmetricKey(PrivateKey privateKey, EncryptedSymmetricKey encryptedSymmetricKey);
        byte[] EncryptWithPublicKey(PublicKey publicKey, byte[] data);
        byte[] DecryptWithPrivateKey(PrivateKey privateKey, byte[] data);
        byte[] EncryptWithKey(KeyBase key, byte[] data);
        byte[] DecryptWithKey(KeyBase key, byte[] data);
    }

    public class AsymmetricEncryptor : IAsymmetricEncryptor
    {
        private const int MIN_KEY_SIZE = 384;

        public EncryptedSymmetricKey EncryptSymmetricKey(PublicKey publicKey, SymmetricKey key)
        {
            return new EncryptedSymmetricKey(GetCryptoServiceProvider(publicKey.Value).Encrypt(key.Value, true));
        }

        public SymmetricKey DecryptSymmetricKey(PrivateKey privateKey, EncryptedSymmetricKey data)
        {
            return new SymmetricKey(GetCryptoServiceProvider(privateKey.Value).Decrypt(data.Value, true));
        }

        private RSACryptoServiceProvider GetCryptoServiceProvider(byte[] cspBlob)
        {
            var parameters = new CspParameters();
            parameters.Flags = CspProviderFlags.UseMachineKeyStore;
            var cryptoServiceProvider = new RSACryptoServiceProvider(MIN_KEY_SIZE, parameters);
            cryptoServiceProvider.ImportCspBlob(cspBlob);
            return cryptoServiceProvider;
        }

        public byte[] EncryptWithPublicKey(PublicKey publicKey, byte[] data)
        {
            return EncryptWithKey(publicKey, data);
        }

        public byte[] DecryptWithPrivateKey(PrivateKey privateKey, byte[] data)
        {
            return DecryptWithKey(privateKey, data);
        }

        public byte[] EncryptWithKey(KeyBase key, byte[] data)
        {
            return GetCryptoServiceProvider(key.Value).Encrypt(data, true);
        }

        public byte[] DecryptWithKey(KeyBase key, byte[] data)
        {
            return GetCryptoServiceProvider(key.Value).Decrypt(data, true);
        }
    }

    public interface ISymmetricEncryptor
    {
        byte[] Encrypt(byte[] bytes, SymmetricKey key, InitializationVector iv);
        byte[] Decrypt(byte[] bytes, SymmetricKey key, InitializationVector iv);
        EncryptedPrivateKey EncryptWithPassword(string password, InitializationVector iv, PrivateKey data);
        PrivateKey DecryptWithPassword(string password, InitializationVector iv, EncryptedPrivateKey data);
        void Encrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv);
        void Decrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv);
    }

    public class SymmetricEncryptor : ISymmetricEncryptor
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
            var keyBytes = new Rfc2898DeriveBytes(password, Salt, Iterations).GetBytes(KeySize);
            return new SymmetricKey(keyBytes);
        }

        private byte[] EncryptOrDecrypt(byte[] bytes, SymmetricKey key, InitializationVector iv, bool decrypt)
        {
            using (var aes = new AesCryptoServiceProvider { Key = key.Value, IV = iv.Value })
            {
                using (var cryptoTransform = decrypt ? aes.CreateDecryptor() : aes.CreateEncryptor())
                {
                    return cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                }
            }
        }

        private void EncryptOrDecrypt(Stream inputStream, Stream outputStream, SymmetricKey key, InitializationVector iv, bool decrypt)
        {
            using (var aes = new AesCryptoServiceProvider { Key = key.Value, IV = iv.Value })
            {
                using (var cryptoTransform = decrypt ? aes.CreateDecryptor() : aes.CreateEncryptor())
                {
                    using (var cryptoStream = new CryptoStream(inputStream, cryptoTransform, CryptoStreamMode.Read))
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
            var bufferSize = 4096;
            var buffer = new byte[bufferSize];
            var bytesRead = 0;
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

    public interface IRandomProvider
    {
        int NextInt(int maximum);
    }

    public class RandomProvider : IRandomProvider
    {
        private RandomNumberGenerator random = RandomNumberGenerator.Create();

        public int NextInt()
        {
            var bytes = new byte[4];
            random.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public bool NextBool()
        {
            var bytes = new byte[1];
            random.GetBytes(bytes);
            if (bytes[0] % 2 == 0)
            {
                return false;
            }
            return true;
        }

        public int NextInt(int maximum)
        {
            var numValidBytes = (int)Math.Ceiling(Math.Log(maximum, 2));
            var mask = (int)((long)Math.Pow(2, numValidBytes) - 1);
            var bytes = new byte[4];
            int r;
            random.GetBytes(bytes);
            r = mask & BitConverter.ToInt32(bytes, 0);
            while (r >= maximum)
            {
                random.GetBytes(bytes);
                r = mask & BitConverter.ToInt32(bytes, 0);
            }
            return r;
        }

        public int Next(int minValue, int maxValue)
        {
            var lower = minValue;
            var higher = maxValue;
            var percent = GetPercent();
            return (int)(((higher - lower) * percent) + lower);
        }

        public void NextBytes(byte[] bytes)
        {
            random.GetBytes(bytes);
        }

        public double NextDouble(double lowest, double highest)
        {
            var percent = GetPercent();
            return ((highest - lowest) * percent) + lowest;
        }

        public float NextFloat(float lowest, float highest)
        {
            var percent = GetPercent();
            return (float)(((highest - lowest) * percent) + lowest);
        }

        private double GetPercent()
        {
            var buffer = new byte[sizeof(UInt64)];
            UInt64 rand;
            random.GetNonZeroBytes(buffer);
            rand = BitConverter.ToUInt64(buffer, 0);
            return (rand / (double)UInt64.MaxValue);
        }

    }

    public class ByteSaltProvider
    {
        private static RandomProvider randomNumber = new RandomProvider();

        public byte[] Salt(byte[] data, int saltLength)
        {
            var saltedBytes = new byte[saltLength];
            var saltedData = new byte[data.Length + saltLength];
            randomNumber.NextBytes(saltedBytes);
            saltedBytes.CopyTo(saltedData, 0);
            data.CopyTo(saltedData, saltLength);
            return saltedData;
        }

        public byte[] Unsalt(byte[] data, int saltLength)
        {
            var unsaltedData = new byte[data.Length - saltLength];
            Array.ConstrainedCopy(data, saltLength, unsaltedData, 0, unsaltedData.Length);
            return unsaltedData;
        }
    }

    public class SerializationService
    {
        private BinaryFormatter _binaryFormatter;

        public SerializationService()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public byte[] Serialize(object objectToSerialize)
        {
            return Serialize(objectToSerialize, 0);
        }

        public byte[] Serialize(object objectToSerialize, int offsetBufferSize)
        {
            using (var stream = new MemoryStream())
            {
                _binaryFormatter.Serialize(stream, objectToSerialize);
                var returnData = new byte[stream.Length + offsetBufferSize];
                stream.Position = 0;
                stream.Read(returnData, offsetBufferSize, (int)stream.Length);
                return returnData;
            }
        }

        public object Deserialize(byte[] unsaltedData)
        {
            using (var stream = new MemoryStream(unsaltedData))
            {
                return _binaryFormatter.Deserialize(stream);
            }
        }
    }

}
