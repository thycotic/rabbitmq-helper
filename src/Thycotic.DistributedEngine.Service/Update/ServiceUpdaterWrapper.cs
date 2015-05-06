using System.IO;
using System.Threading;
using Thycotic.DistributedEngine.Logic.EngineToServer;
using Thycotic.Logging;
using Thycotic.WindowsService.Bootstraper;

namespace Thycotic.DistributedEngine.Service.Update
{
    /// <summary>
    /// Simple ServiceUpdaterWrapper
    /// </summary>
    public class ServiceUpdaterWrapper : IServiceUpdaterWrapper
    {
        private readonly IUpdateBus _updateBus;

        private bool _updating;
        private readonly object _syncRoot = new object();

        private readonly ILogWriter _log = Log.Get(typeof(ServiceUpdaterWrapper));

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUpdaterWrapper"/> class.
        /// </summary>
        /// <param name="updateBus">The update bus.</param>
        public ServiceUpdaterWrapper(IUpdateBus updateBus)
        {
            _updateBus = updateBus;
        }

        /// <summary>
        /// Applies the latest update.
        /// </summary>
        public void ApplyLatestUpdate()
        {
            lock (_syncRoot)
            {
                if (_updating)
                {
                    _log.Warn("Update already in progress");
                    return;
                }

                _updating = true;
            }

            var msiPath = Path.Combine(Path.GetTempPath(), "Thycotic.DistributedEngine.Service-Update..msi");

            _updateBus.GetUpdate(msiPath);

            var cts = new CancellationTokenSource();

            var serviceUpdater = new ServiceUpdater(cts, Directory.GetCurrentDirectory(),
                "Thycotic.DistributedEngine.Service", msiPath);

            serviceUpdater.Update();

            _updating = false;
        }
    }
}