using System.Security.Cryptography;
using Thycotic.AppCore.Cryptography;

namespace Thycotic.SecretServerEngine2.Security
{
    /// <summary>
    /// Local key provider
    /// </summary>
    public class LocalKeyProvider : ILocalKeyProvider
    {
        /// <summary>
        /// Gets the engine key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        public void GetKeys(out PublicKey publicKey, out PrivateKey privateKey)
        {
            const int rsaSecurityKeySize = 2048;
            const CspProviderFlags flags = CspProviderFlags.UseMachineKeyStore;
            var cspParameters = new CspParameters { Flags = flags };

            using (var provider = new RSACryptoServiceProvider(rsaSecurityKeySize, cspParameters))
            {
                privateKey = new PrivateKey(provider.ExportCspBlob(true));
                publicKey = new PublicKey(provider.ExportCspBlob(false));
            }
        }
    }
}