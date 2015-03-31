using Thycotic.Encryption;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Local key provider
    /// </summary>
    public class AuthenticatedCommunicationKeyProvider : SymmetricKeyPair, IAuthenticatedCommunicationKeyProvider
    {
    }
}