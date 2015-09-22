using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Request;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Response;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.DistributedEngine.Service.Configuration;
using Thycotic.DistributedEngine.Service.Update;
using Thycotic.Logging;
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
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly IResponseBus _responseBus;
        private readonly IUpdateInitializer _updateInitializer;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly ILogWriter _log = Log.Get(typeof(HeartbeatRunner));
        private Task _pumpTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatRunner" /> class.
        /// </summary>
        /// <param name="heartbeatConfigurationProvider">The heartbeat configuration provider.</param>
        /// <param name="engineService">The engine service.</param>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        /// <param name="responseBus">The response bus.</param>
        /// <param name="updateInitializer">The UpdateInitializer.</param>
        public HeartbeatRunner(IHeartbeatConfigurationProvider heartbeatConfigurationProvider, 
            EngineService engineService, 
            IEngineIdentificationProvider engineIdentificationProvider, 
            IResponseBus responseBus,
            IUpdateInitializer updateInitializer)
        {
            Contract.Requires<ArgumentNullException>(heartbeatConfigurationProvider != null);
            Contract.Requires<ArgumentNullException>(engineService != null);
            Contract.Requires<ArgumentNullException>(engineIdentificationProvider != null);
            Contract.Requires<ArgumentNullException>(responseBus != null);
            Contract.Requires<ArgumentNullException>(updateInitializer != null);
            _heartbeatConfigurationProvider = heartbeatConfigurationProvider;
            _engineService = engineService;
            _engineIdentificationProvider = engineIdentificationProvider;
            _responseBus = responseBus;
            _updateInitializer = updateInitializer;
        }

       [SuppressMessage("Microsoft.Contracts", "TestAlwaysEvaluatingToAConstant", Justification = "Bogus warning about _cts.Token.IsCancellationRequested")]
        private void Pump()
        {
            Contract.Assume(ReleaseInformationHelper.Version != null);

            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            _log.Info("Calling back to the server");

            var version = typeof(EngineService).Assembly.GetName().Version;
            var request = new EngineHeartbeatRequest
            {
                IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                OrganizationId = _engineIdentificationProvider.OrganizationId,
                Version = version.ToString(),
                LastActivity = DateTime.UtcNow
            };

            var response = _responseBus.Execute<EngineHeartbeatRequest, EngineHeartbeatResponse>(request);

            if (!response.Success)
            {
                throw new ConfigurationErrorsException(response.ErrorMessage);
            }

            if (_cts.Token.IsCancellationRequested)
            {
                return;
            }

            if (response.UpdateNeeded)
            {
                _updateInitializer.ApplyLatestUpdate();
            }
            else
            {
                //the configuration has not changed since it was last consumed
                if (response.LastConfigurationUpdated <= _engineService.IoCConfigurator.LastConfigurationConsumed)
                {
                    return;
                }

                _log.Info("Configuration changed. Recycling...");

                _engineService.IoCConfigurator.TryAssignConfiguration(response.NewConfiguration);

                _engineService.Recycle();
            }
        }



        private void WaitPumpAndSchedule()
        {
            if (_cts.Token.IsCancellationRequested)
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

                    _log.Error("Failed to callback to the server. Recycling", task.Exception);

                    _engineService.Recycle(true);
                }, _cts.Token)
                //schedule
                .ContinueWith(task => WaitPumpAndSchedule(), _cts.Token);
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Info(string.Format("Callback runner is starting for every {0} seconds...", _heartbeatConfigurationProvider.HeartbeatIntervalSeconds));

            Task.Factory.StartNew(WaitPumpAndSchedule);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Contract.Assume(_pumpTask != null);
            _log.Info("Callback runner is stopping...");

            _cts.Cancel();

            _log.Debug("Waiting for callback runner to complete...");

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