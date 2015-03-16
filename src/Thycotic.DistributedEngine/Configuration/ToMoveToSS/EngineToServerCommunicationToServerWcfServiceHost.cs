using Autofac;
using Thycotic.Wcf;

namespace Thycotic.DistributedEngine.Configuration.ToMoveToSS
{
    /// <summary>
    /// Engine to server communication service host
    /// </summary>
    public class EngineToServerCommunicationToServerWcfServiceHost : ServiceHostBase<EngineToServerCommunicationWcfService, IEngineToServerCommunicationWcfService>, IStartable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineToServerCommunicationToServerWcfServiceHost"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public EngineToServerCommunicationToServerWcfServiceHost(string connectionString)
            : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EngineToServerCommunicationToServerWcfServiceHost"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        public EngineToServerCommunicationToServerWcfServiceHost(string connectionString, string thumbprint)
            : base(connectionString, thumbprint)
        {
        }


    }
}
