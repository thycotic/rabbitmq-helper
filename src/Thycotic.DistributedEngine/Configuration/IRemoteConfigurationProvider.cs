using System.Collections.Generic;

namespace Thycotic.DistributedEngine.Configuration
{
    /// <summary>
    /// Interface for a remote configuration provider
    /// </summary>
    public interface IRemoteConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetConfiguration();
    }
}