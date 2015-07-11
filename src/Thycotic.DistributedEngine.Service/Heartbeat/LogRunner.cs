using System;
using System.Configuration;
using System.Diagnostics.Contracts;
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
    /// Log runner
    /// </summary>
    public class LogRunner : IStartable, IDisposable
    {
        private readonly IRecentLogEntryProvider _recentLogEntryProvider;
        private readonly IEngineIdentificationProvider _engineIdentificationProvider;
        private readonly IResponseBus _responseBus;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private const int IntervalSeconds = 10;
        private const int BufferSize = 25;

        private Task _pumpTask;
        private bool _flush;

        private readonly ILogWriter _log = Log.Get(typeof(LogRunner));

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatRunner" /> class.
        /// </summary>
        /// <param name="recentLogEntryProvider">The recent log entry provider.</param>
        /// <param name="engineIdentificationProvider">The engine identification provider.</param>
        /// <param name="responseBus">The rest communication provider.</param>
        public LogRunner(IRecentLogEntryProvider recentLogEntryProvider,
            IEngineIdentificationProvider engineIdentificationProvider,
            IResponseBus responseBus)
        {
            Contract.Requires<ArgumentNullException>(recentLogEntryProvider != null);
            Contract.Requires<ArgumentNullException>(engineIdentificationProvider != null);
            Contract.Requires<ArgumentNullException>(responseBus != null);
            _recentLogEntryProvider = recentLogEntryProvider;
            _engineIdentificationProvider = engineIdentificationProvider;
            _responseBus = responseBus;
        }

        private void Pump()
        {
            if (!_flush && _cts.Token.IsCancellationRequested)
            {
                return;
            }

            if (!_flush && _recentLogEntryProvider.Count < BufferSize)
            {
                _log.Debug("Insufficient log entries to warrant log pump");
                return;
            }

            _log.Info("Sending engine log to server");

            _log.Debug("Extracting log entries from provider");
            var logEntries = _recentLogEntryProvider.GetEntries().Select(MapLogEntries).ToList();

            //clear the recent log entries
            _log.Debug("Clearing log entries from provider");
            _recentLogEntryProvider.Clear();

            _log.Debug(string.Format("Breaking log dump into batches of {0}", BufferSize));

            var batches = logEntries.Count / BufferSize;

            //there isn't a full batch left, send it anyway
            if (logEntries.Count % BufferSize > 0) batches++;

            Enumerable.Range(0, batches).ToList().ForEach(i =>
            {
                var batch = logEntries.Skip(i * BufferSize).Take(BufferSize).ToArray();

                var request = new EngineLogRequest
                {
                    IdentityGuid = _engineIdentificationProvider.IdentityGuid,
                    OrganizationId = _engineIdentificationProvider.OrganizationId,
                    Version = ReleaseInformationHelper.Version.ToString(),
                    LogEntries = batch
                };

                _log.Debug(string.Format("Sending batch {0}", i));

                var response = _responseBus.Execute<EngineLogRequest, EngineLogResponse>(request);

                if (!response.Success)
                {
                    throw new ConfigurationErrorsException(response.ErrorMessage);
                }
            });
        }

        private void WaitPumpAndSchedule()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _pumpTask = Task.Factory
                //wait
                .StartNew(() => _cts.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(10)), _cts.Token)
                //pump
                .ContinueWith(task => Pump(), _cts.Token)
                //react to errors
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        _log.Error("Failed to send log to server", task.Exception);
                    }
                }, _cts.Token)
                //schedule
                .ContinueWith(task => WaitPumpAndSchedule(), _cts.Token);
        }

        /// <summary>
        /// Perform once-off startup processing.
        /// </summary>
        public void Start()
        {
            _log.Info(string.Format("Log runner starting for every {0} seconds with a buffer size of {1}...", IntervalSeconds, BufferSize));

            Task.Factory.StartNew(WaitPumpAndSchedule);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Contract.Assume(_pumpTask != null);
            _log.Info("Log runner is stopping...");

            _cts.Cancel();

            _log.Debug("Waiting for log runner to complete...");

            try
            {
                _pumpTask.Wait();
            }
            catch (AggregateException ex)
            {
                ex.InnerExceptions.Where(e => !(e is TaskCanceledException)).ToList().ForEach(e => _log.Error(e.Message, e));
            }

            _log.Info("Flushing remaining log");
            _flush = true;
            Pump();
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