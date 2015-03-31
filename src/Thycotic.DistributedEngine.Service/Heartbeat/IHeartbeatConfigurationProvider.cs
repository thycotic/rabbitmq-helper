namespace Thycotic.DistributedEngine.Heartbeat
{
    /// <summary>
    /// Interface for heartheat configuration provider
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