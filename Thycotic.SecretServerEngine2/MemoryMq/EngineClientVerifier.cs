using System.IdentityModel.Selectors;
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
        /// Validates the specified agentkey. This an override for <see cref="UserNamePasswordValidator"/>.
        /// It just calls <see cref="EngineClientVerifier.Validate(string)"/>
        /// </summary>
        /// <param name="agentkey">The agentkey.</param>
        /// <param name="ignored">Ignored parameter.</param>
        public override void Validate(string agentkey, string ignored)
        {
        }

        /// <summary>
        /// Validates the specified agentkey.
        /// </summary>
        /// <param name="agentkey">The agentkey.</param>
        public void Validate(string agentkey)
        {
            _log.Info(string.Format("Engine with key {0} connected", agentkey));
            _log.Warn("Engine validation is inactive");
        }
    }
}