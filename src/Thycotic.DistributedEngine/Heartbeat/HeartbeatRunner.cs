using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.DistributedEngine.Security;
using Thycotic.DistributedEngine.Web.Common.Logging;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.Encryption;
using Thycotic.Logging;
using Thycotic.Logging.LogTail;
using Thycotic.Logging.Models;
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
        private readonly IHeartbeatConfigurationProvider _heartbeatConfigurationProvider;
        private readonly EngineService _engineService;
        private readonly IRecentLogEntryProvider _recentLogEntryProvider;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly ILocalKeyProvider _localKeyProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IEngineToServerCommunicationProvider _engineToServerCommunicationProvider;
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
        /// <param name="localKeyProvider">The local key provider.</param>
        /// <param name="objectSerializer">The object serializer.</param>
        /// <param name="engineToServerCommunicationProvider">The rest communication provider.</param>
        public HeartbeatRunner(IHeartbeatConfigurationProvider heartbeatConfigurationProvider, EngineService engineService, IRecentLogEntryProvider recentLogEntryProvider, IEngineIdentificationProvider engineIdentificationProvider, ILocalKeyProvider localKeyProvider, IObjectSerializer objectSerializer, IEngineToServerCommunicationProvider engineToServerCommunicationProvider)
        {
            _heartbeatConfigurationProvider = heartbeatConfigurationProvider;
            _engineService = engineService;
            _recentLogEntryProvider = recentLogEntryProvider;
            _engineIdentificationProvider = engineIdentificationProvider;
            _localKeyProvider = localKeyProvider;
            _objectSerializer = objectSerializer;
            _engineToServerCommunicationProvider = engineToServerCommunicationProvider;
        }

        private void Pump()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _log.Info("Heart beating back to server");

            var logEntries = _recentLogEntryProvider.GetEntries().Select(MapLogEntries).ToArray();

            var request = new EngineHeartbeatRequest
            {
                IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                OrganizationId = _engineIdentificationProvider.OrganizationId,
                PublicKey = Convert.ToBase64String(_localKeyProvider.PublicKey.Value),
                Version = ReleaseInformationHelper.GetVersionAsDouble(),
                LastActivity = DateTime.UtcNow,
                LogEntries = logEntries
            };

            var response = _engineToServerCommunicationProvider.SendHeartbeat(request);

            if (!response.Success)
            {
                throw new ConfigurationErrorsException(response.ErrorMessage);
            }

            //clear the recent log entries
            _recentLogEntryProvider.Clear();

            //the configuration has not changed since it was last consumed
            if (response.LastConfigurationUpdated <= _engineService.IoCConfigurator.LastConfigurationConsumed) return;

            var saltProvider = new ByteSaltProvider();

            var asymmetricEncryptor = new AsymmetricEncryptor();
            var decryptedConfiguration = asymmetricEncryptor.DecryptWithKey(_localKeyProvider.PrivateKey,
                response.NewConfiguration);
            var unsaltedConfiguration = saltProvider.Unsalt(decryptedConfiguration, MessageEncryption.SaltLength);

            var newConfiguration = _objectSerializer.ToObject<Dictionary<string, string>>(unsaltedConfiguration);

            _engineService.Stop();

            Console.WriteLine();
            Console.WriteLine("@ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @ @");
            Console.WriteLine();

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
                .StartNew(() => _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(_heartbeatConfigurationProvider.HeartbeatIntervalSeconds)), _cts.Token)
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
            _log.Info(string.Format("SendHeartbeat is starting for every {0} seconds...", _heartbeatConfigurationProvider.HeartbeatIntervalSeconds));

            Task.Factory.StartNew(WaitPumpAndSchedule);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _log.Info("SendHeartbeat is stopping...");

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

        private static EngineLogEntry MapLogEntries(LogEntry logEntry)
        {
            return new EngineLogEntry
            {
                Id = logEntry.Id,
                Date = logEntry.Date,
                UserId = logEntry.UserId,
                ServiceRole = logEntry.ServiceRole,
                Correlation = logEntry.Correlation,
                Context = logEntry.Context,
                Thread = logEntry.Thread,
                Level = logEntry.Level,
                Logger = logEntry.Logger,
                Message = logEntry.Message,
                Exception = logEntry.Exception
            };
        }
    }
}