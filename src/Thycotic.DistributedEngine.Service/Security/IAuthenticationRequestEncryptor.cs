using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Security;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Authentication encryptor
    /// </summary>
    [ContractClass(typeof(AuthenticationRequestEncryptorContract))]
    public interface IAuthenticationRequestEncryptor : IRequestEncryptor<PublicKey, PrivateKey>
    {
    }
    
    /// <summary>
    /// Contract for IAuthenticationRequestEncryptor
    /// </summary>
    [ContractClassFor(typeof(IAuthenticationRequestEncryptor))]
    public abstract class AuthenticationRequestEncryptorContract : IAuthenticationRequestEncryptor
    {
        /// <summary>
        /// Interface for an envelope Decrypter
        /// </summary>
        /// <param name="decryptionKey">Decryption Key</param>
        /// <param name="bytes">Bytes</param>
        /// <returns>Decrypted Byte Array</returns>
        public byte[] Decrypt(PrivateKey decryptionKey, byte[] bytes)
        {
            Contract.Requires<ArgumentNullException>(decryptionKey!=null);
            Contract.Requires<ArgumentNullException>(bytes!=null);

            return default(byte[]);
        }

        /// <summary>
        /// Interface for an envelope Encrypter
        /// </summary>
        /// <param name="encryptionKey">Encyption Key</param>
        /// <param name="bytes">Bytes</param>
        /// <returns>Encrypted Byte Array</returns>
        public byte[] Encrypt(PublicKey encryptionKey, byte[] bytes)
        {
            Contract.Requires<ArgumentNullException>(encryptionKey != null);
            Contract.Requires<ArgumentNullException>(bytes != null);

            return default(byte[]);
        }
    }
}
