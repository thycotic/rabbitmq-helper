using System;
using System.Diagnostics;
using System.ServiceProcess;
using Autofac;
using Thycotic.Logging;
using Thycotic.MemoryMq.Pipeline.Service.Configuration;

namespace Thycotic.MemoryMq.Pipeline.Service
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

        private readonly ILogWriter _log = Log.Get(typeof(PipelineService));
        private ServiceProcessInstaller _serviceProcessInstaller;
        private ServiceInstaller _serviceInstaller;
        private LogCorrelation _correlation;


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
                _log.Info("Pipeline starting...");

                base.OnStart(args);

                BringUp();

                _log.Info("Pipeline started");
            }
            
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            using (LogContext.Create("Stopping"))
            {
                _log.Info("Pipeline stopping...");

                base.OnStop();

                TearDown();

                _log.Info("Pipeline stopped");
            }
        }

        private void InitializeComponent()
        {
            this._serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this._serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // _serviceProcessInstaller
            // 
            this._serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this._serviceProcessInstaller.Password = null;
            this._serviceProcessInstaller.Username = null;
            // 
            // _serviceInstaller
            // 
            this._serviceInstaller.DisplayName = "Thycotic.MemoryMq.Pipeline.Service";
            this._serviceInstaller.ServiceName = "Thycotic.MemoryMq.Pipeline.Service";
            this._serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;

        }
    }
}
