using System.Diagnostics;

namespace Thycotic.WindowsService.Bootstraper
{
    public class ProcessRunner : IProcessRunner
    {
        public Process Start(ProcessStartInfo processInfo)
        {
            return Process.Start(processInfo);
        }
    }
}
