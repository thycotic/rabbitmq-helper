using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.EngineToServerCommunication.Logging;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.Configuration;
using Thycotic.Logging;
using Thycotic.Logging.LogTail;
using Thycotic.Logging.Models;
using Thycotic.Utility;

namespace Thycotic.DistributedEngine.Service.Heartbeat
{
    /// <summary>
    /// Startup message writer. Mostly to ensure Autofac is working properly.
    /// </summary>
    public class HeartbeatRunner : IStartable, IDisposable
    {
        private readonly IHeartbeatConfigurationProvider _heartbeatConfigurationProvider;
        private readonly EngineService _engineService;
        private readonly IRecentLogEntryProvider _recentLogEntryProvider;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly IResponseBus _responseBus;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly ILogWriter _log = Log.Get(typeof(HeartbeatRunner));
        private Task _pumpTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatRunner" /> class.
        /// </summary>
        /// <param name="heartbeatConfigurationProvider">The heartbeat configuration provider.</param>
        /// <param name="engineService">The engine service.</param>
        /// <param name="recentLogEntryProvider">The recent log entry provider.</param>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        /// <param name="responseBus">The rest communication provider.</param>
        public HeartbeatRunner(IHeartbeatConfigurationProvider heartbeatConfigurationProvider, 
            EngineService engineService, 
            IRecentLogEntryProvider recentLogEntryProvider, 
            IEngineIdentificationProvider engineIdentificationProvider, 
            IResponseBus responseBus)
        {
            _heartbeatConfigurationProvider = heartbeatConfigurationProvider;
            _engineService = engineService;
            _recentLogEntryProvider = recentLogEntryProvider;
            _engineIdentificationProvider = engineIdentificationProvider;
            _responseBus = responseBus;            
        }

        private void Pump()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _log.Info("Heart beating back to server");

            var request = new EngineHeartbeatRequest
            {
                IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                OrganizationId = _engineIdentificationProvider.OrganizationId,
                Version = ReleaseInformationHelper.GetVersionAsDouble(),
                LastActivity = DateTime.UtcNow
            };

            var response = _responseBus.Execute<EngineHeartbeatRequest, EngineHeartbeatResponse>(request);

            if (!response.Success)
            {
                throw new ConfigurationErrorsException(response.ErrorMessage);
            }

            if (_cts.IsCancellationRequested)
            {
                return;
            }

            //the configuration has not changed since it was last consumed
            if (response.LastConfigurationUpdated <= _engineService.IoCConfigurator.LastConfigurationConsumed) return;

            _log.Info("Configuration changed. Recycling...");

            _engineService.IoCConfigurator.TryAssignConfiguration(response.NewConfiguration);

            _engineService.Recycle();
        }



        private void WaitPumpAndSchedule()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _pumpTask = Task.Factory
                //wait
                .StartNew(() => _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(_heartbeatConfigurationProvider.HeartbeatIntervalSeconds)), _cts.Token)
                //pump
                .ContinueWith(task => Pump(), _cts.Token)
                //react to errors
                .ContinueWith(task =>
                {
                    if (task.Exception == null) return;

                    //exit and let service manager restart the service
                    _log.Error("Failed to send heart beat to server. Recycling...", task.Exception);
                    _engineService.Recycle(true);

                })
                //schedule
                .ContinueWith(task => WaitPumpAndSchedule(), _cts.Token);
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Info(string.Format("Heartbeat runner is starting for every {0} seconds...", _heartbeatConfigurationProvider.HeartbeatIntervalSeconds));

            Task.Factory.StartNew(WaitPumpAndSchedule);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _log.Info("Heartbeat runner is stopping...");

            _cts.Cancel();

            _log.Debug("Waiting for heartbeat runner to complete...");

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