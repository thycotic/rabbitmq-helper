using System.IdentityModel.Selectors;
using Thycotic.Logging;

namespace Thycotic.SecretServerEngine2.MemoryMq
{
    public class EngineClientVerifier : UserNamePasswordValidator, IEngineClientVerifier
    {
        private readonly ILogWriter _log = Log.Get(typeof(EngineClientVerifier));

        public override void Validate(string agentkey, string ignored)
        {
            _log.Info(string.Format("Engine with key {0} connected", agentkey));
            _log.Warn("Engine validation is inactive");


        }
    }
}