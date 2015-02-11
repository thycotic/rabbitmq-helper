using System;
using System.Security.Cryptography;


namespace Thycotic.MessageQueueClient
{
    class KeyExchange
    {
        #region From AppCore
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
                CspParameters parameters = new CspParameters();
                parameters.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(MIN_KEY_SIZE, parameters);
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

        public interface IRandomProvider
        {
            int NextInt(int maximum);
        }

        public class RandomProvider : IRandomProvider
        {
            private RandomNumberGenerator random = RandomNumberGenerator.Create();

            public int NextInt()
            {
                byte[] bytes = new byte[4];
                random.GetBytes(bytes);
                return BitConverter.ToInt32(bytes, 0);
            }

            public bool NextBool()
            {
                byte[] bytes = new byte[1];
                random.GetBytes(bytes);
                if (bytes[0] % 2 == 0)
                {
                    return false;
                }
                return true;
            }

            public int NextInt(int maximum)
            {
                int numValidBytes = (int)Math.Ceiling(Math.Log(maximum, 2));
                int mask = (int)((long)Math.Pow(2, numValidBytes) - 1);
                byte[] bytes = new byte[4];
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
                int lower = minValue;
                int higher = maxValue;
                double percent = GetPercent();
                return (int)(((higher - lower) * percent) + lower);
            }

            public void NextBytes(byte[] bytes)
            {
                random.GetBytes(bytes);
            }

            public double NextDouble(double lowest, double highest)
            {
                double percent = GetPercent();
                return ((highest - lowest) * percent) + lowest;
            }

            public float NextFloat(float lowest, float highest)
            {
                double percent = GetPercent();
                return (float)(((highest - lowest) * percent) + lowest);
            }

            private double GetPercent()
            {
                byte[] buffer = new byte[sizeof(UInt64)];
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
                byte[] saltedBytes = new byte[saltLength];
                byte[] saltedData = new byte[data.Length + saltLength];
                randomNumber.NextBytes(saltedBytes);
                saltedBytes.CopyTo(saltedData, 0);
                data.CopyTo(saltedData, saltLength);
                return saltedData;
            }

            public byte[] Unsalt(byte[] data, int saltLength)
            {
                byte[] unsaltedData = new byte[data.Length - saltLength];
                Array.ConstrainedCopy(data, saltLength, unsaltedData, 0, unsaltedData.Length);
                return unsaltedData;
            }
        }

        public void CreateSymmetricKeyAndIv(out SymmetricKey symmetricKey, out InitializationVector initializationVector)
        {
            int AesKeySize = 256;
            int IvSize = 128;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = IvSize;
                aes.KeySize = AesKeySize;
                aes.GenerateIV();
                aes.GenerateKey();
                symmetricKey = new SymmetricKey(aes.Key);
                initializationVector = new InitializationVector(aes.IV);
            }
        }
        #endregion

        public void Blah(string publicKey, string version)
        {
            const int SALT_LENGTH = 8;

            var saltProvider = new ByteSaltProvider();

            SymmetricKey symmetricKey;
            InitializationVector initializationVector;
            CreateSymmetricKeyAndIv(out symmetricKey, out initializationVector);
            //_openAgentConnectionProvider.AddClient(new Client
            //{
            //    PublicKey = publicKey,
            //    Name = name,
            //    SymmetricKey = symmetricKey.Value,
            //    InitalizationVector = initializationVector.Value
            //}, callback);
            AsymmetricEncryptor asymmetricEncryptor = new AsymmetricEncryptor();
            byte[] saltedSymmetricKey = saltProvider.Salt(symmetricKey.Value, SALT_LENGTH);
            byte[] encryptedSymmetricKey = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedSymmetricKey);
            byte[] saltedInitializationVector = saltProvider.Salt(initializationVector.Value, SALT_LENGTH);
            byte[] encryptedInitializationVector = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedInitializationVector);
            double versionNum;
            bool canParse = double.TryParse(version, out versionNum);
        }
    }
}
