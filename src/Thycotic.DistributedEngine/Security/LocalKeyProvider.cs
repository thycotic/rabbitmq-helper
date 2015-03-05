using System;
using System.Security.Cryptography;
using Thycotic.AppCore.Cryptography;
using Thycotic.Logging;
using Thycotic.Utility.Security;

namespace Thycotic.DistributedEngine.Security
{
    /// <summary>
    /// Local key provider
    /// </summary>
    public class LocalKeyProvider : ILocalKeyProvider
    {

        private readonly Lazy<Tuple<PublicKey, PrivateKey>> _pair;

        private readonly ILogWriter _log = Log.Get(typeof(LocalKeyProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalKeyProvider"/> class.
        /// </summary>
        public LocalKeyProvider()
        {
            _pair = new Lazy<Tuple<PublicKey, PrivateKey>>(() =>
            {
                _log.Debug("Generating local public private pair");

                //TODO: Review key size 4096 with Kevin -dkk
                const int rsaSecurityKeySize = 4096;
                const CspProviderFlags flags = CspProviderFlags.UseMachineKeyStore;
                var cspParameters = new CspParameters {Flags = flags};

                using (var provider = new RSACryptoServiceProvider(rsaSecurityKeySize, cspParameters))
                {
                    var publicKey = new PublicKey(provider.ExportCspBlob(false));
                    var privateKey = new PrivateKey(provider.ExportCspBlob(true));

                    return new Tuple<PublicKey, PrivateKey>(publicKey, privateKey);
                }
            });
        }

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>
        /// The public key.
        /// </value>
        public PublicKey PublicKey
        {
            get { return _pair.Value.Item1; }
        }

        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        public PrivateKey PrivateKey
        {
            get { return _pair.Value.Item2; }
        }
    }
}