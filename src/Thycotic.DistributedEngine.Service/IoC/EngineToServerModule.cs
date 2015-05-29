using System;
using Autofac;
using Thycotic.DistributedEngine.Service.EngineToServer;
using Thycotic.DistributedEngine.Service.Security;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class EngineToServerModule : Module
    {
        private readonly Func<string, string> _configurationProvider;
        private readonly EngineService _engineService;

        //private readonly ILogWriter _log = Log.Get(typeof(HeartbeatModule));


        public EngineToServerModule(Func<string, string> configurationProvider, EngineService engineService)
        {
            _configurationProvider = configurationProvider;
            _engineService = engineService;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new AuthenticatedCommunicationKeyProvider
            {
                SymmetricKey =
                    new SymmetricKey(Convert.FromBase64String(_configurationProvider(ConfigurationKeys.Engine.SymmetricKey))),
                InitializationVector =
                    new InitializationVector(
                        Convert.FromBase64String(_configurationProvider(ConfigurationKeys.Engine.InitializationVector)))
            }).As<IAuthenticatedCommunicationKeyProvider>().SingleInstance();
           
            builder.RegisterModule(new HeartbeatModule(_configurationProvider, _engineService));


            builder.RegisterType<ResponseBus>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UpdateBus>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
