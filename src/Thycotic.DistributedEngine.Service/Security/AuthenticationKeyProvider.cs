using System;
using Thycotic.Encryption;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Service.Security
{
    /// <summary>
    /// Local key provider
    /// </summary>
    public class AuthenticationKeyProvider : IAuthenticationKeyProvider
    {
        private readonly Lazy<AsymmetricKeyPair> _pair;

        private readonly ILogWriter _log = Log.Get(typeof(AuthenticationKeyProvider));

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationKeyProvider"/> class.
        /// </summary>
        public AuthenticationKeyProvider()
        {
            _pair = new Lazy<AsymmetricKeyPair>(() =>
            {
                using (LogContext.Create("Key generation"))
                {
                    _log.Info("Generating local public/private pair...");

                    return EngineToServerCommunication.Engine.Security.AuthenticationKeyProvider.GenerateKeyPair();
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
            get { return _pair.Value.PublicKey; }
        }

        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>
        /// The private key.
        /// </value>
        public PrivateKey PrivateKey
        {
            get { return _pair.Value.PrivateKey; }
        }
    }
}