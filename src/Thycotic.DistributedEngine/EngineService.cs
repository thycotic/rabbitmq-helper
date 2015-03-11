using System;
using System.Diagnostics;
using System.ServiceProcess;
using Autofac;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.Wrappers;

namespace Thycotic.DistributedEngine
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


        private readonly ILogWriter _log = Log.Get(typeof(EngineService));
        private LogCorrelation _correlation;


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
                if (!IoCConfigurator.TryGetAndAssignConfiguration())
                {
                    _log.Info("Engine is not enabled/configured. Existing...");
                    return;
                }

                // Build the container to finalize registrations and prepare for object resolution.
                _ioCContainer = IoCConfigurator.Build(this, _startConsuming);

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

            if (_startConsuming)
            {
                _log.Info("Stopping all consumers...");
                var consumerFactory = _ioCContainer.Resolve<ConsumerWrapperFactory>();

                //clean up the consumers in the factory
                consumerFactory.Dispose();
            }

            _ioCContainer.Dispose();
            _ioCContainer = null;
        }

        private void BringUp()
        {
            _correlation = LogCorrelation.Create();

            ResetIoCContainer();

            ConfigureIoC();
        }

        private void TearDown()
        {
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
            OnStart(new string []{ });
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            using (LogContext.Create("Starting"))
            {
                _log.Info("Engine starting...");

                base.OnStart(args);

                BringUp();

                _log.Info("Engine started");
            }
            
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            using (LogContext.Create("Stopping"))
            {
                _log.Info("Engine stopping...");

                base.OnStop();

                TearDown();

                _log.Info("Engine stopped");
            }
        }
    }
}
