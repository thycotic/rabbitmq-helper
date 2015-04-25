using System.Collections.Generic;
using System.Linq;
using Thycotic.Discovery.Core.Results;

namespace Thycotic.DistributedEngine.Logic.Areas.Discovery
{
    /// <summary>
    /// Discovery Log Extensions
    /// </summary>
    public static class DiscoveryLogExtensions
    {
        private const string TOKEN_SEPARATOR = "%$#@@||@@#$%";
        private const int MaxLogByteSize = 4000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public static List<DiscoveryLog> Truncate(this List<DiscoveryLog> logs)
        {
            List<DiscoveryLog> returnLogs;
            var logBytes = GetBytes(string.Join(TOKEN_SEPARATOR, logs.Select(log => log.Message)));
            if (logBytes.Length < MaxLogByteSize)
            {
                returnLogs = logs;
            }
            else
            {
                var logString = GetString(logBytes) + "...(Logs truncated due to size limitations)";
                var logsInternal = new StringSplitter().Split(TOKEN_SEPARATOR, logString).ToList();
                var logList = logsInternal.Select(log => new DiscoveryLog { Message = log }).ToList();
                returnLogs = logList;
            }
            return returnLogs;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[MaxLogByteSize];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, MaxLogByteSize);
            return new string(chars);
        }
    }
}
