using System;
using System.IdentityModel.Selectors;
using System.Runtime.Remoting.Channels;
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
            _log.Info(string.Format("Engine client with key {0} connected", clientKey));

            try
            {
                EnsureClientIsEnabled(clientKey);

                _log.Info("Client is enabled and will be accepted.");
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Engine client with key {0} is not enabled. Client will be rejected.", clientKey), ex);
                throw;
            }
        }

        private void EnsureClientIsEnabled(string clientKey)
        {
            _log.Info(string.Format("Validating whether the engine client with key {0} is enabled.", clientKey));

            _log.Warn("Engine validation is not live. Allowing all clients.");
            //throw new SecurityException("Client is not allowed");
        }
    }
}