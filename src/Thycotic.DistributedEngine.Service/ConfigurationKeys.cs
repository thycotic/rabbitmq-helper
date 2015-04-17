namespace Thycotic.DistributedEngine.Service
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigurationKeys
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public class EngineToServerCommunication
        {
            /// <summary>
            /// The connection string
            /// </summary>
            public const string ConnectionString = "EngineToServerCommunication.ConnectionString";

            /// <summary>
            /// The use SSL
            /// </summary>
            public const string UseSsl = "EngineToServerCommunication.UseSsl";

            /// <summary>
            /// The exchange identifier
            /// </summary>
            public const string ExchangeId = "EngineToServerCommunication.ExchangeId";

            /// <summary>
            /// The organization identifier
            /// </summary>
            public const string OrganizationId = "EngineToServerCommunication.OrganizationId";

            
        }
    }
}
