namespace Thycotic.DistributedEngine.Service.Heartbeat
{
    class HeartbeatConfigurationProvider : IHeartbeatConfigurationProvider
    {
        public int HeartbeatIntervalSeconds { get; set; }
    }
}
