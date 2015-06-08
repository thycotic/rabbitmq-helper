using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Thycotic.DistributedEngine.Service.Configuration;
using Thycotic.DistributedEngine.Service.Update;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.Wrappers;

namespace Thycotic.DistributedEngine.Service
{
    /// <summary>
    /// Engine windows service
    /// </summary>
    public class EngineService : ServiceBase
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

        private readonly bool _startConsuming;

        private IContainer _ioCContainer;

        private CancellationTokenSource _runningTokenSource;

        private LogCorrelation _correlation;

        private readonly ILogWriter _log = Log.Get(typeof(EngineService));

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineService"/> class.
        /// </summary>
        public EngineService() : this(true, new IoCConfigurator()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineService" /> class.
        /// </summary>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        public EngineService(bool startConsuming) : this(startConsuming, new IoCConfigurator()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineService" /> class.
        /// </summary>
        /// <param name="startConsuming">if set to <c>true</c> [start consuming].</param>
        /// <param name="ioCConfigurator">The iio c configurator.</param>
        public EngineService(bool startConsuming, IIoCConfigurator ioCConfigurator)
        {
            _startConsuming = startConsuming;
            IoCConfigurator = ioCConfigurator;
            ConfigureLogging();
        }


        private static void ConfigureLogging()
        {
            Trace.TraceInformation("Configuring logging...");
            Log.Configure();
            Trace.TraceInformation("Attaching recent events appender...");
            Log.AttachRecentEventsMemoryAppender();
        }

        /// <summary>
        /// Configures inversion of control.
        /// </summary>
        public void ConfigureIoC()
        {
            _log.Debug("Configuring IoC...");

            try
            {
                bool updateNeeded;
                if (!IoCConfigurator.TryGetAndAssignConfiguration(out updateNeeded))
                {
                    _log.Info("Engine is not enabled/configured. Existing...");
                }

                // BuildAll the container to finalize registrations and prepare for object resolution.
                _ioCContainer = IoCConfigurator.BuildAll(this, !updateNeeded && _startConsuming);

                //notify any hooks that IoC is configured
                if (IoCContainerConfigured != null)
                {
                    IoCContainerConfigured(this, _ioCContainer);
                }

                if (!updateNeeded)
                {
                    return;
                }

                ApplyUpdate();
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Failed to configure IoC because {0}", ex.Message), ex);
                throw;
            }
        }

        private void ApplyUpdate()
        {
            var serviceUpdaterWrapper = _ioCContainer.Resolve<IUpdateInitializer>();

            serviceUpdaterWrapper.ApplyLatestUpdate();
        }

        private void ResetIoCContainer()
        {
            if (_ioCContainer == null)
            {
                return;
            }

            _log.Debug("Cleaning up IoC container");

            try
            {


                if (_startConsuming && _ioCContainer.IsRegistered<ConsumerWrapperFactory>())
                {
                    _log.Info("Stopping all consumers...");
                    var consumerFactory = _ioCContainer.Resolve<ConsumerWrapperFactory>();

                    //clean up the consumers in the factory
                    consumerFactory.Dispose();
                }

            }
            catch (Exception ex)
            {
                _log.Warn("Failed to properly clean up consumer factory", ex);
            }

            try
            {

                _ioCContainer.Dispose();
            }

            catch (Exception ex)
            {
                _log.Warn("Failed to properly clean up the IoC container", ex);
            }

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
                    _log.Error("Failed to bring up engine", task.Exception);
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
                    _log.Info("Engine starting...");

                    BringUp();

                    _log.Info("Engine started");
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to start engine", ex);
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
                    _log.Info("Engine stopping...");

                    TearDown();

                    _log.Info("Engine stopped");
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to stop engine", ex);
                }
            }
        }

        /// <summary>
        /// Recycles this instance.
        /// </summary>
        public void Recycle(bool removeInstanceConfiguration = false)
        {
            try
            {
                TearDown();

                if (removeInstanceConfiguration)
                {
                    _log.Info("Removing instance configuration");
                    IoCConfigurator.TryAssignConfiguration(null);
                }

                BringUp();
            }
            catch (Exception ex)
            {
                _log.Error("Failed to recycle engine", ex);
                Environment.Exit(-1);
            }
        }

    }
}
