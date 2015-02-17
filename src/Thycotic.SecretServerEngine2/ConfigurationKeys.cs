namespace Thycotic.SecretServerEngine2
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
