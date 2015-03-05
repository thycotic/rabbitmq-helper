using System;
using Autofac;
using Thycotic.AppCore;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.Heartbeat;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.Logging;
using Thycotic.Utility.Serialization;
using Module = Autofac.Module;

namespace Thycotic.DistributedEngine.IoC
{
    class HeartbeatModule : Module
    {
        private readonly EngineService _engineService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private readonly Func<string, string> _configurationProvider;

        private readonly ILogWriter _log = Log.Get(typeof(HeartbeatModule));
        

        public HeartbeatModule(EngineService engineService, IDateTimeProvider dateTimeProvider, IEngineIdentificationProvider engineIdentificationProvider, ILocalKeyProvider localKeyProvider, IObjectSerializer objectSerializer, IRestCommunicationProvider restCommunicationProvider, Func<string, string> configurationProvider)
        {
            _engineService = engineService;
            _dateTimeProvider = dateTimeProvider;
            _engineIdentificationProvider = engineIdentificationProvider;
            _localKeyProvider = localKeyProvider;
            _objectSerializer = objectSerializer;
            _restCommunicationProvider = restCommunicationProvider;
            _configurationProvider = configurationProvider;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            _log.Debug("Initializing heartbeat...");

            var heartbeatIntervalSeconds =
                Convert.ToInt32(_configurationProvider(MessageQueue.Client.ConfigurationKeys.HeartbeatIntervalSeconds));

            builder.Register(context => new HeartbeatRunner(_engineService, _dateTimeProvider, _engineIdentificationProvider, _localKeyProvider, _objectSerializer, _restCommunicationProvider, heartbeatIntervalSeconds)).As<IStartable>().SingleInstance();
        }
    }
}
