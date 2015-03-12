using System.Security.Cryptography;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// AsymmetricEncryptor
    /// </summary>
    public class AsymmetricEncryptor : IAsymmetricEncryptor
    {
        private const int MIN_KEY_SIZE = 384;

        /// <summary>
        /// Encrypts the symmetric key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public EncryptedSymmetricKey EncryptSymmetricKey(PublicKey publicKey, SymmetricKey key)
        {
            return new EncryptedSymmetricKey(GetCryptoServiceProvider(publicKey.Value).Encrypt(key.Value, true));
        }

        /// <summary>
        /// Decrypts the symmetric key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Encrypts the with public key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] EncryptWithPublicKey(PublicKey publicKey, byte[] data)
        {
            return EncryptWithKey(publicKey, data);
        }

        /// <summary>
        /// Decrypts the with private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] DecryptWithPrivateKey(PrivateKey privateKey, byte[] data)
        {
            return DecryptWithKey(privateKey, data);
        }

        /// <summary>
        /// Encrypts the with key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] EncryptWithKey(KeyBase key, byte[] data)
        {
            return GetCryptoServiceProvider(key.Value).Encrypt(data, true);
        }

        /// <summary>
        /// Decrypts the with key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] DecryptWithKey(KeyBase key, byte[] data)
        {
            return GetCryptoServiceProvider(key.Value).Decrypt(data, true);
        }
    }
}
