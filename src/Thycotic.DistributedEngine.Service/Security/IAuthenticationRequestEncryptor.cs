using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Security;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Authentication encryptor
    /// </summary>
    public interface IAuthenticationRequestEncryptor : IRequestEncryptor<PublicKey, PrivateKey>
    {
    }
}
