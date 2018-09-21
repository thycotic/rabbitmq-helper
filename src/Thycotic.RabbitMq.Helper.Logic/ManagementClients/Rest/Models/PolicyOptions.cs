namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
    /// <summary>
    /// Policy options
    /// </summary>
    public static class PolicyOptions
    {
        /// <summary>
        /// Supported policy applications
        /// </summary>
        public static class PolicyApplications
        {
            /// <summary>
            /// All
            /// </summary>
            public const string All = "all";
            
            /// <summary>
            /// The exchanges
            /// </summary>
            public const string Exchanges = "exchanges";

            /// <summary>
            /// The queues
            /// </summary>
            public const string Queues = "queues";
        }


        /// <summary>
        /// Supported queue master locations
        /// </summary>
        public static class QueueMasterLocation
        {
            /// <summary>
            /// Pick the node hosting the minimum number of bound masters
            /// </summary>
            public const string MinMasters = "min-masters";
            /// <summary>
            /// Pick the node the client that declares the queue is connected to
            /// </summary>
            public const string ClientLocal = "client-local";
            /// <summary>
            /// Pick a random node
            /// </summary>
            public const string Random = "random";
        }
        /// <summary>
        /// Supported federation upstream sets
        /// </summary>
        public static class FederationUpstreamSets
        {
            /// <summary>
            /// All
            /// </summary>
            public const string All = "all";
        }


        /// <summary>
        /// Supported HA sync modes
        /// </summary>
        public static class HaSyncModes
        {
            /// <summary>
            /// This is the default mode. A new queue mirror will not receive existing messages, it will only receive new messages. The new queue mirror will become an exact replica of the master over time, once consumers have drained messages that only exist on the master. If the master queue fails before all unsychronised messages are drained, those messages will be lost. You can fully synchronise a queue manually, refer to unsynchronised mirrors section for details.
            /// </summary>
            public const string Manual = "manual";

            /// <summary>
            /// A queue will automatically synchronise when a new mirror joins.It is worth reiterating that queue synchronisation is a blocking operation.If queues are small, or you have a fast network between RabbitMQ nodes and the ha-sync-batch-size was optimised, this is a good choice.
            /// </summary>
            public const string Automatic = "automatic";
        }

        /// <summary>
        /// Supported HA modes
        /// </summary>
        public static class HaModes
        {
            /// <summary>
            /// Queue is mirrored across all nodes in the cluster. When a new node is added to the cluster, the queue will be mirrored to that node.
            /// This setting is very conservative, mirroring to a quorum(N/2 + 1) of cluster nodes is recommended instead.Mirroring to all nodes will put additional strain on all cluster nodes, including network I/O, disk I/O and disk space usage.
            /// </summary>
            public const string All = "all";

            /// <summary>
            /// Number of queue replicas (master plus mirrors) in the cluster.
            /// A count value of 1 means just the queue master, with no mirrors.If the node running the queue master becomes unavailable, the behaviour depends on queue durability.
            /// A count value of 2 means 1 queue master and 1 queue mirror. If the node running the queue master becomes unavailable, the queue mirror will be automatically promoted to master.In conclusion: NumberOfQueueMirrors = NumberOfNodes - 1.
            /// </summary>
            public const string Exactly = "exactly";

            /// <summary>
            /// The nodes
            /// </summary>
            public const string Nodes = "nodes";
        }
    }
}