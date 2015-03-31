namespace Thycotic.DistributedEngine.Service.Heartbeat
{
    /// <summary>
    /// Interface for heartbeat configuration provider
    /// </summary>
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
}