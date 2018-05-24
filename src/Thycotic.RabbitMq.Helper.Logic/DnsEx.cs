namespace Thycotic.RabbitMq.Helper.Logic
{
    /// <summary>
    /// DNS Extensions
    /// </summary>
    public static class DnsEx
    {
        /// <summary>
        /// Gets the name of the DNS host.
        /// </summary>
        /// <returns></returns>
        public static string GetDnsHostName()
        {
            var hostEntry = System.Net.Dns.GetHostEntry("LocalHost");

         
            var hostname = hostEntry.HostName;

            return hostname;
        }
    }
}