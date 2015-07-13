using System.Diagnostics;

namespace Thycotic.WindowsService.Bootstraper
{
    /// <summary>
    /// Process runner
    /// </summary>
    public class ProcessRunner : IProcessRunner
    {
        /// <summary>
        /// Starts the specified process information.
        /// </summary>
        /// <param name="processInfo">The process information.</param>
        /// <returns></returns>
        public Process Start(ProcessStartInfo processInfo)
        {
            return Process.Start(processInfo);
        }
    }
}
