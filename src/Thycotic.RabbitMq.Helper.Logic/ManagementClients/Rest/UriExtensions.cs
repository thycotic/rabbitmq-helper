using System;
using System.Reflection;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest
{
    /// <summary>
    /// Uri extensions
    /// </summary>
    public static class UriExtensions
    {

        ////https://stackoverflow.com/questions/781205/getting-a-url-with-an-url-encoded-slash
        /// <summary>
        /// Forces the canonical path and query.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Uri WithCanonicalPathAndQuery(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException();
            }

            var paq = uri.PathAndQuery; // need to access PathAndQuery
            var flagsFieldInfo = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            var flags = (ulong)flagsFieldInfo.GetValue(uri);
            flags &= ~(ulong)0x30; // Flags.PathNotCanonical|Flags.QueryNotCanonical
            flagsFieldInfo.SetValue(uri, flags);
            return uri;
        }
    }
}