using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Thycotic.WindowsService.Bootstraper
{
    /// <summary>
    /// Process runner
    /// </summary>
    [ContractClass(typeof(ProcessRunnerContractClass))]
    public interface IProcessRunner
    {
        /// <summary>
        /// Starts the specified process information.
        /// </summary>
        /// <param name="processInfo">The process information.</param>
        /// <returns></returns>
        Process Start(ProcessStartInfo processInfo);
    }

    /// <summary>
    /// Contract for Process Runner.
    /// </summary>
    [ContractClassFor(typeof (IProcessRunner))]
    public abstract class ProcessRunnerContractClass : IProcessRunner
    {

        /// <summary>
        /// Starts the specified process information.
        /// </summary>
        /// <param name="processInfo">The process information.</param>
        /// <returns></returns>
        public Process Start(ProcessStartInfo processInfo)
        {
            Contract.Requires<ArgumentNullException>(processInfo != null);
            return default(Process);
        }
    }
}