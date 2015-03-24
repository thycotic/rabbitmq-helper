using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Security;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Authenticated communication encryptor
    /// </summary>
    public interface IAuthenticatedCommunicationRequestEncryptor : IEnvelopeEncryptor<SymmetricKeyPair, SymmetricKeyPair>
    {
    }
}
