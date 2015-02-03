using System.IdentityModel.Selectors;

namespace Thycotic.SecretServerAgent2.MemoryMq
{
    internal class AgentVerifier : UserNamePasswordValidator
    {
        public override void Validate(string agentkey, string ignored)
        {
            //TODO: Validate
        }
    }
}