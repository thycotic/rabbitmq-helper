using System.IdentityModel.Selectors;
using Thycotic.Logging;

namespace Thycotic.SecretServerEngine2.MemoryMq
{
    internal class EngineVerifier : UserNamePasswordValidator
    {
        private readonly ILogWriter _log = Log.Get(typeof(EngineVerifier));

        public override void Validate(string agentkey, string ignored)
        {
            _log.Info(string.Format("Engine with key {0} connected", agentkey));
            _log.Warn("Engine validation is inactive");


        }
    }
}