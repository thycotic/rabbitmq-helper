using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.Heartbeat
{
    /// <summary>
    /// Startup message writer. Mostly to ensure Autofac is working properly.
    /// </summary>
    public class HeartbeatRunner : IStartable, IDisposable
    {
        private readonly EngineService _engineService;
        private readonly int _heartbeatIntervalSeconds;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly ILogWriter _log = Log.Get(typeof (HeartbeatRunner));
        private Task _pumpTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeartbeatRunner" /> class.
        /// </summary>
        /// <param name="engineService">The engine service.</param>
        /// <param name="heartbeatIntervalSeconds">The heartbeat configuration provider.</param>
        public HeartbeatRunner(EngineService engineService, int heartbeatIntervalSeconds)
        {
            _engineService = engineService;
            _heartbeatIntervalSeconds = heartbeatIntervalSeconds;
        }

        private void Pump()
        {
            if (_cts.IsCancellationRequested)
            {
                return;
            }

            _log.Info("Heart beating back to server");

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