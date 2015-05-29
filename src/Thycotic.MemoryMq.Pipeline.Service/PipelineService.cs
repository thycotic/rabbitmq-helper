using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.Logging;
using Thycotic.MemoryMq.SiteConnector.Service.Configuration;

namespace Thycotic.MemoryMq.SiteConnector.Service
{
    /// <summary>
    /// Engine windows service
    /// </summary>
    public class PipelineService : ServiceBase
    {
        /// <summary>
        /// Occurs when the IoC container configured.
        /// </summary>
        public EventHandler<IContainer> IoCContainerConfigured;

        /// <summary>
        /// Gets the IoC configurator.
        /// </summary>
        /// <value>
        /// The IoC configurator.
        /// </value>
        public IIoCConfigurator IoCConfigurator { get; private set; }

        private IContainer _ioCContainer;

        private CancellationTokenSource _runningTokenSource;

        private LogCorrelation _correlation;

        private readonly ILogWriter _log = Log.Get(typeof(PipelineService));


        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineService"/> class.
        /// </summary>
        public PipelineService() : this(new IoCConfigurator()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineService" /> class.
        /// </summary>
        /// <param name="ioCConfigurator">The iio c configurator.</param>
        public PipelineService(IIoCConfigurator ioCConfigurator)
        {
            IoCConfigurator = ioCConfigurator;
            ConfigureLogging();
        }


        private static void ConfigureLogging()
        {
            Trace.TraceInformation("Configuring logging...");
            Log.Configure();
            //don't attach the recent events appender since nothing is draining them
            //Trace.TraceInformation("Attaching recent events appender...");
            //Log.AttachRecentEventsMemoryAppender();
        }

        /// <summary>
        /// Configures inversion of control.
        /// </summary>
        public void ConfigureIoC()
        {
            _log.Debug("Configuring IoC...");

            try
            {
                // BuildAll the container to finalize registrations and prepare for object resolution.
                _ioCContainer = IoCConfigurator.BuildAll(this);

                //notify any hooks that IoC is configured
                if (IoCContainerConfigured != null)
                {
                    IoCContainerConfigured(this, _ioCContainer);
                }

                _log.Debug("Configuring IoC complete");

            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Failed to configure IoC because {0}", ex.Message), ex);
                throw;
            }
        }

        private void ResetIoCContainer()
        {
            if (_ioCContainer == null) return;

            _log.Debug("Cleaning up IoC container");

            _ioCContainer.Dispose();
            _ioCContainer = null;
        }

        private void BringUp()
        {
            _correlation = LogCorrelation.Create();

            _runningTokenSource = new CancellationTokenSource();

            ResetIoCContainer();

            var configured = false;

            Task.Factory.StartNew(() =>
            {
                //keep trying to configure until success or the running source is cancelled
                while (!configured && !_runningTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        ConfigureIoC();

                        configured = true;
                    }
                    catch
                    {
                        _log.Warn("Could not configure, trying in 30 seconds");
                        Task.Delay(TimeSpan.FromSeconds(30)).Wait(_runningTokenSource.Token);
                    }
                }
            }, _runningTokenSource.Token).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    _log.Error("Failed to bring up pipeline", task.Exception);
                }
            });
        }

        private void TearDown()
        {
            if (_runningTokenSource != null)
            {
                _runningTokenSource.Dispose();
            }

            ResetIoCContainer();

            if (_correlation != null)
            {
                _correlation.Dispose();
            }
        }

        /// <summary>
        /// Starts the specified arguments.
        /// </summary>
        public void Start()
        {
            OnStart(new string[] { });
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            using (LogContext.Create("Starting"))
            {
                try
                {

                    _log.Info("Pipeline starting...");

                    BringUp();

                    _log.Info("Pipeline started");
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to start pipeline", ex);
                }
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            using (LogContext.Create("Stopping"))
            {
                try
                {
                    _log.Info("Pipeline stopping...");

                    TearDown();

                    _log.Info("Pipeline stopped");
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to stop pipeline", ex);
                }
            }
        }
    }
}
