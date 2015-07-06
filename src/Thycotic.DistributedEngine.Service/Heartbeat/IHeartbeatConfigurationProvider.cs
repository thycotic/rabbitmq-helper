using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Service.Heartbeat
{
    /// <summary>
    /// Interface for heartbeat configuration provider
    /// </summary>
    [ContractClass(typeof(HeartbeatConfigurationProviderContract))]
    public interface IHeartbeatConfigurationProvider
    {
        /// <summary>
        /// Gets or sets the heartbeat interval seconds.
        /// </summary>
        /// <value>
        /// The heartbeat interval seconds.
        /// </value>
        int HeartbeatIntervalSeconds { get; set; }
    }

    /// <summary>
    /// Contract for IHeartbeatConfigurationProvider
    /// </summary>
    [ContractClassFor(typeof (IHeartbeatConfigurationProvider))]
    public abstract class HeartbeatConfigurationProviderContract : IHeartbeatConfigurationProvider
    {
        /// <summary>
        /// Gets or sets the heartbeat interval seconds.
        /// </summary>
        /// <value>
        /// The heartbeat interval seconds.
        /// </value>
        public int HeartbeatIntervalSeconds { get; set; }
    }
}