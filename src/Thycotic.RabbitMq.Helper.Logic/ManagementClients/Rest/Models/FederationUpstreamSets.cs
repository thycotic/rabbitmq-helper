namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models
{
    /// <summary>
    /// Supported federation up-stream sets
    /// </summary>
    public static class FederationUpstreamSets
    {

        /// <summary>
        /// All.  Federate all the built-in exchanges except for the default exchange, with a single upstream
        /// </summary>
        public const string All = "all";
    }
}
