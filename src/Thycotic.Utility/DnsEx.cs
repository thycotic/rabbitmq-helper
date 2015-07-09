using System.Diagnostics.Contracts;

namespace Thycotic.Utility
{
    /// <summary>
    /// DNS specific helpers
    /// </summary>
    public static class DnsEx
    {
        /// <summary>
        /// Gets the name of the DNS host.
        /// </summary>
        /// <returns></returns>
        public static string GetDnsHostName()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            var hostEntry = System.Net.Dns.GetHostEntry("LocalHost");

            Contract.Assume(hostEntry != null);

            var hostname = hostEntry.HostName;

            Contract.Assume(!string.IsNullOrEmpty(hostname));
            return hostname;
        }
    }
}
