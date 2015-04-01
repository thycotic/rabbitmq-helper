namespace Thycotic.MemoryMq
{
    /// <summary>
    /// Configuration keys
    /// </summary>
    public class ConfigurationKeys
    {
        /// <summary>
        /// The connection string
        /// </summary>
        public const string ConnectionString = "Pipeline.ConnectionString";

        /// <summary>
        /// The user name
        /// </summary>
        public const string UserName = "Pipeline.UserName";

        /// <summary>
        /// The password
        /// </summary>
        public const string Password = "Pipeline.Password";

        /// <summary>
        /// Whether or not to use SSL
        /// </summary>
        public static string UseSsl = "Pipeline.UseSsl";

        /// <summary>
        /// The thumbprint
        /// </summary>
        public static string Thumbprint = "Pipeline.UseSSL";
    }
}
