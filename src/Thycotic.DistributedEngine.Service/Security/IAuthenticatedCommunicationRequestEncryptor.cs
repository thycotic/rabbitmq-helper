using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Security;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{

    /// <summary>
    /// Authenticated communication encryptor
    /// </summary>
    [ContractClass(typeof(AuthenticatedCommunicationRequestEncryptorContract))]
    public interface IAuthenticatedCommunicationRequestEncryptor : IRequestEncryptor<SymmetricKeyPair, SymmetricKeyPair>
    {
    }


    /// <summary>
    /// Contract for IAuthenticatedCommunicationRequestEncryptor
    /// </summary>
    [ContractClassFor(typeof(IAuthenticatedCommunicationRequestEncryptor))]
    public abstract class AuthenticatedCommunicationRequestEncryptorContract : IAuthenticatedCommunicationRequestEncryptor
    {

        /// <summary>
        /// Interface for an envelope Decrypter
        /// </summary>
        /// <param name="decryptionKey">Decryption Key</param>
        /// <param name="bytes">Bytes</param>
        /// <returns>Decrypted Byte Array</returns>
        public byte[] Decrypt(SymmetricKeyPair decryptionKey, byte[] bytes)
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
        public byte[] Encrypt(SymmetricKeyPair encryptionKey, byte[] bytes)
        {
            Contract.Requires<ArgumentNullException>(encryptionKey != null);
            Contract.Requires<ArgumentNullException>(bytes != null);

            return default(byte[]);
        }
    }

}
