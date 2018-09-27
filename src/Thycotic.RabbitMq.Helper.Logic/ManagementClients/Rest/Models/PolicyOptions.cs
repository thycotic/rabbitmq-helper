namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
    /// <summary>
    /// Component options
    /// </summary>
    public static class ComponentParameterOptions
    {
        /// <summary>
        /// Options for federation
        /// </summary>
        public static class Federation
        {
            /// <summary>
            /// The component
            /// </summary>
            public const string Component = "federation-upstream";

            /// <summary>
            /// Supported federation parameter keys
            /// </summary>
            public static class ParameterKeys
            {
                /// <summary>
                /// The ack mode
                /// </summary>
                public const string AckMode = "ack-mode";

                /// <summary>
                /// The pre fetch count
                /// </summary>
                public const string PreFetchCount = "prefetch-count";

                /// <summary>
                /// The reconnect delay
                /// </summary>
                public const string ReconnectDelay = "reconnect-delay";

                /// <summary>
                /// The trust user identifier
                /// </summary>
                public const string TrustUserId = "trust-user-id";

                /// <summary>
                /// The URI
                /// </summary>
                public const string Uri = "uri";

                /// <summary>
                /// The expires
                /// </summary>
                public const string Expires = "expires";
            }

            /// <summary>
            /// Supported ack modes
            /// </summary>
            public static class AckModes
            {
                /// <summary>
                /// Messages are acknowledged to the upstream broker after they have been confirmed downstream. Handles network errors and broker failures without losing messages. The slowest option, and the default.
                /// </summary>
                public const string OnConfirm = "on-confirm";
                /// <summary>
                /// Messages are acknowledged to the upstream broker after they have been published downstream. Handles network errors without losing messages, but may lose messages in the event of broker failures.
                /// </summary>
                public const string OnPublish = "on-publish";
                /// <summary>
                /// Message acknowledgements are not used. The fastest option, but may lose messages in the event of network or broker failures.
                /// </summary>
                public const string NoAck = "no-ack";

            }
        }
    }


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
            /// All. Federate all the built-in exchanges except for the default exchange, with a single upstream
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

        /// <summary>
        /// Supported policy parameter keys
        /// </summary>
        public static class ParameterKeys
        {
            /// <summary>
            /// The queue master locator
            /// </summary>
            public const string QueueMasterLocator = "queue-master-locator";

            /// <summary>
            /// The ha mode
            /// </summary>
            public const string HaMode = "ha-mode";

            /// <summary>
            /// The ha parameters
            /// </summary>
            public const string HaParams = "ha-params";

            /// <summary>
            /// The ha synchronize mode
            /// </summary>
            public const string HaSyncMode = "ha-sync-mode";

            /// <summary>
            /// The ha synchronize batch size
            /// </summary>
            public const string HaSyncBatchSize = "ha-sync-batch-size";

            /// <summary>
            /// The federation upstream set
            /// </summary>
            public const string FederationUpstreamSet = "federation-upstream-set";
        }
    }
}