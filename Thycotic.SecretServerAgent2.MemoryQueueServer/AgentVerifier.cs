using System.IdentityModel.Selectors;

namespace Thycotic.SecretServerAgent2.MemoryQueueServer
{
    internal class AgentVerifier : UserNamePasswordValidator
    {
        public override void Validate(string agentkey, string ignored)
        {
            //TODO: Validate
        }
    }
}