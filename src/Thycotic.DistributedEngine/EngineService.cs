﻿using System;
using System.ServiceProcess;
using Autofac;
using Thycotic.DistributedEngine.Configuration;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Engine windows service
    /// </summary>
    public class EngineService : ServiceBase
    {
        /// <summary>
        /// Gets or sets the io c container.
        /// </summary>
        /// <value>
        /// The io c container.
        /// </value>
        public IContainer IoCContainer { get; private set; }

        private readonly bool _startConsuming;
        private readonly IIoCConfigurator _iioCConfigurator;

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
        /// <param name="iioCConfigurator">The iio c configurator.</param>
        public EngineService(bool startConsuming, IIoCConfigurator iioCConfigurator)
        {
            _startConsuming = startConsuming;
            _iioCConfigurator = iioCConfigurator;
            ConfigureLogging();
        }


        private static void ConfigureLogging()
        {
            Log.Configure();
        }

        /// <summary>
        /// Configures inversion of control.
        /// </summary>
        public void ConfigureIoC()
        {
            _log.Debug("Configuring IoC...");

            try
            {
                ResetIoCContainer();

                if (!_iioCConfigurator.TryGetRemoteConfiguration())
                {
                    _log.Info("Engine is not enabled/configured. Existing...");
                    //TODO: Maybe retry later -dkk
                    return;
                }

                // Build the container to finalize registrations and prepare for object resolution.
                IoCContainer = _iioCConfigurator.Build(_startConsuming);

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
            if (IoCContainer == null) return;

            _log.Debug("Cleaning up IoC container");

            IoCContainer.Dispose();
        }

        private void BringUp()
        {
            _correlation = LogCorrelation.Create();

            ConfigureIoC();
        }

        private void TearDown()
        {
            ResetIoCContainer();

            _correlation.Dispose();
        }

        /// <summary>
        /// Starts the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Start(string[] args)
        {
            OnStart(args);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            using (LogContext.Create("Starting"))
            {
                base.OnStart(args);

                BringUp();
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            using (LogContext.Create("Stopping"))
            {
                base.OnStop();

                TearDown();
            }
        }
    }
}