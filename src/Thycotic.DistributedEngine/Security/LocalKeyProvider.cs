using System.Security.Cryptography;
using Thycotic.AppCore.Cryptography;

namespace Thycotic.SecretServerEngine.Security
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
            //TODO: Review key size 4096 with Kevin -dkk
            const int rsaSecurityKeySize = 4096;
            const CspProviderFlags flags = CspProviderFlags.UseMachineKeyStore;
            var cspParameters = new CspParameters { Flags = flags };

            using (var provider = new RSACryptoServiceProvider(rsaSecurityKeySize, cspParameters))
            {
                publicKey = new PublicKey(provider.ExportCspBlob(false));
                privateKey = new PrivateKey(provider.ExportCspBlob(true));
            }
        }
    }
}