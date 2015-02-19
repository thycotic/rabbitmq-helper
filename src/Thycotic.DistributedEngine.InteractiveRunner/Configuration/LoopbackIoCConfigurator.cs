using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.Logic;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackIoCConfigurator : IoCConfigurator
    {
        public LoopbackIoCConfigurator() : base(new LoopbackRestCommunicationProvider(),  new LoopbackConfigurationProvider())
        {
            
        }

        private LoopbackIoCConfigurator(IRestCommunicationProvider restCommunicationProvider, IRemoteConfigurationProvider remoteConfigurationProvider)
            : base(restCommunicationProvider, remoteConfigurationProvider)
        {
        }
    }
}