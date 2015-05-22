using System.Diagnostics;

namespace Thycotic.WindowsService.Bootstraper
{
    public interface IProcessRunner
    {
        Process Start(ProcessStartInfo processInfo);
    }
}