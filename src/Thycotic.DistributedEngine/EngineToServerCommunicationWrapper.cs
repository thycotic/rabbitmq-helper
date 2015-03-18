using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.Logging;
using Thycotic.Wcf;

namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Wrapper for users of <see cref="IEngineToServerCommunicationWcfService"/> channels
    /// </summary>
    public class EngineToServerCommunicationWrapper
    {
        /// <summary>
        /// The channel
        /// </summary>
        protected IEngineToServerCommunicationWcfService Channel { get; private set; }

        private readonly ILogWriter _log = Log.Get(typeof(EngineToServerCommunicationWrapper));

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineConfigurationBus"/> class.
        /// </summary>
        /// <param name="connectionString">The URI.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public EngineToServerCommunicationWrapper(string connectionString, bool useSsl)
        {
            var uri = new Uri(connectionString);

            switch (uri.Scheme)
            {
                case "net.tcp":
                    _log.Info(string.Format("Using Net/TCP channel to {0}", connectionString));
                    Channel = NetTcpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(connectionString, useSsl);
                    break;
                case "http":
                case "https":
                    _log.Info(string.Format("Using HTTP channel to {0}", connectionString));
                    //Channel = HttpChannelFactory.CreateChannel<IEngineToServerCommunicationWcfService>(connectionString, useSsl);
                    break;
                default:
                    throw new NotSupportedException("Requested schema does not have a supported channel");
            }
        }
    }
}