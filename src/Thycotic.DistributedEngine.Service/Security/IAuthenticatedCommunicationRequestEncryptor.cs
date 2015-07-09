using System;
using System.Diagnostics.Contracts;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Security;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{

    /// <summary>
    /// Authenticated communication encryptor
    /// </summary>
    public interface IAuthenticatedCommunicationRequestEncryptor : IRequestEncryptor<SymmetricKeyPair, SymmetricKeyPair>
    {
    }
}
