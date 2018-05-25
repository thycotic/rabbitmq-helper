using System.IO;

namespace Thycotic.RabbitMq.Helper.Logic.IO
{
    /// <summary>
    /// RabbitMq configuration path sanitizer
    /// </summary>
    public static class RabbitMqConfigurationPathSanitizer
    {
        /// <summary>
        /// Sanitizes the specified path. Directory separators are doubled: C:\temp comes C:\\temp
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string Sanitize(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar.ToString(),
                string.Format("{0}{0}", Path.DirectorySeparatorChar));
        }
    }
}
