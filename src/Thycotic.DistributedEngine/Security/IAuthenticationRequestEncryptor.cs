using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Security;
using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Authentication encryptor
    /// </summary>
    public interface IAuthenticationRequestEncryptor : IRequestEncryptor<PublicKey, PrivateKey>
    {
    }
}
