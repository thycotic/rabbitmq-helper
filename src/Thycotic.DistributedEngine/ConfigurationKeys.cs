namespace Thycotic.DistributedEngine
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public static class ConfigurationKeys
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public class RemoteConfiguration
        {
            /// <summary>
            /// The connection string
            /// </summary>
            public const string ConnectionString = "RemoteConfiguration.ConnectionString";

            /// <summary>
            /// The exchange identifier
            /// </summary>
            public const string ExchangeId = "RemoteConfiguration.ExchangeId";

            /// <summary>
            /// The organization identifier
            /// </summary>
            public const string OrganizationId = "RemoteConfiguration.OrganizationId";

            /// <summary>
            /// The friendly name of the engine
            /// </summary>
            public const string FriendlyName = "RemoteConfiguration.FriendlyName";
            
            /// <summary>
            /// The identity unique identifier
            /// </summary>
            public const string IdentityGuid = "RemoteConfiguration.IdentityGuid";
        }
    }
}
