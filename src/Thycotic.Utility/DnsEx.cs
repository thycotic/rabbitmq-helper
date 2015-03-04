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
            return System.Net.Dns.GetHostEntry("LocalHost").HostName;
        }
    }
}
