using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Security;
using Thycotic.DistributedEngine.Web.Common;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.Logging;
using Thycotic.Utility;
using Thycotic.Utility.Security;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.Heartbeat
{
    /// <summary>
    /// Startup message writer. Mostly to ensure Autofac is working properly.
    /// </summary>
    public class HeartbeatRunner : IStartable, IDisposable
    {
        private readonly EngineService _engineService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IRestCommunicationProvider _restCommunicationProvider;
        private readonly int _heartbeatIntervalSeconds;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly ILogWriter _log = Log.Get(typeof(HeartbeatRunner));
        private Task _pumpTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatRunner" /> class.
        /// </summary>
        /// <param name="engineService">The engine service.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="restCommunicationProvider">The rest communication provider.</param>
        /// <param name="heartbeatIntervalSeconds">The heartbeat configuration provider.</param>
        public HeartbeatRunner(EngineService engineService, IDateTimeProvider dateTimeProvider, IEngineIdentificationProvider engineIdentificationProvider, ILocalKeyProvider localKeyProvider, IObjectSerializer objectSerializer, IRestCommunicationProvider restCommunicationProvider, int heartbeatIntervalSeconds)
        {
            _engineService = engineService;
            _dateTimeProvider = dateTimeProvider;
            _engineIdentificationProvider = engineIdentificationProvider;
            _localKeyProvider = localKeyProvider;
            _objectSerializer = objectSerializer;
            _restCommunicationProvider = restCommunicationProvider;
            _heartbeatIntervalSeconds = heartbeatIntervalSeconds;
        }

        private void Pump()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _log.Info("Heart beating back to server");
            
            var uri = _restCommunicationProvider.GetEndpointUri(EndPoints.EngineWebService.Prefix,
                    EndPoints.EngineWebService.Actions.Heartbeat);

            var request = new EngineHeartbeatRequest
            {
                
                IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                PublicKey = Convert.ToBase64String(_localKeyProvider.PublicKey.Value),
                Version = ReleaseInformationHelper.GetVersionAsDouble(),
                LastActivity = _dateTimeProvider.Now
            };

            var response = _restCommunicationProvider.Post<EngineHeartbeatResponse>(uri, request);

            if (!response.Success)
            {
                throw new ConfigurationErrorsException(response.ErrorMessage);
            }

            //the configuration has not changed since it was last consumed
            if (response.LastConfigurationUpdated <= _engineService.IoCConfigurator.LastConfigurationConsumed) return;

            var saltProvider = new ByteSaltProvider();

            var asymmetricEncryptor = new AsymmetricEncryptor();
            var decryptedConfiguration = asymmetricEncryptor.DecryptWithKey(_localKeyProvider.PrivateKey,
                response.NewConfiguration);
            var unsaltedConfiguration = saltProvider.Unsalt(decryptedConfiguration, MessageEncryption.SaltLength);

            var newConfiguration = _objectSerializer.ToObject<Dictionary<string, string>>(unsaltedConfiguration);

            _engineService.Stop();

            _engineService.IoCConfigurator.TryAssignConfiguration(newConfiguration);

            _engineService.Start();
        }

        private void WaitPumpAndSchedule()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _pumpTask = Task.Factory
                //wait
                .StartNew(() => _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(_heartbeatIntervalSeconds)), _cts.Token)
                //pump
                .ContinueWith(task => Pump(), _cts.Token)
                //schedule
                .ContinueWith(task => WaitPumpAndSchedule(), _cts.Token);
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Info(string.Format("Heartbeat is starting for every {0} seconds...", _heartbeatIntervalSeconds));

            Task.Factory.StartNew(WaitPumpAndSchedule);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _log.Info("Heartbeat is stopping...");

            _cts.Cancel();

            _log.Debug("Waiting for heartbeat to complete...");

            try
            {
                _pumpTask.Wait();
            }
            catch (AggregateException ex)
            {
                ex.InnerExceptions.Where(e => !(e is TaskCanceledException)).ToList().ForEach(e => _log.Error(e.Message, e));
            }
        }
    }
}