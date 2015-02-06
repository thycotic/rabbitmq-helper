using System.IdentityModel.Selectors;
using System.Security;
using Thycotic.Logging;

namespace Thycotic.SecretServerEngine2.MemoryMq
{
    /// <summary>
    /// Engine client verifier
    /// </summary>
    public class EngineClientVerifier : UserNamePasswordValidator, IEngineClientVerifier
    {
        private readonly ILogWriter _log = Log.Get(typeof(EngineClientVerifier));

        /// <summary>
        /// Validates the specified clientKey. This an override for <see cref="UserNamePasswordValidator"/>.
        /// It just calls <see cref="EngineClientVerifier.Validate(string)"/>
        /// </summary>
        /// <param name="clientKey">The clientKey.</param>
        /// <param name="ignored">Ignored parameter.</param>
        public override void Validate(string clientKey, string ignored)
        {
            Validate(clientKey);
        }

        /// <summary>
        /// Validates the specified clientKey.
        /// </summary>
        /// <param name="clientKey">The clientKey.</param>
        public void Validate(string clientKey)
        {
            _log.Info(string.Format("Engine with key {0} connected", clientKey));
            //_log.Warn("Engine validation is inactive");
            throw new SecurityException("Client is not allowed");
        }
    }
}