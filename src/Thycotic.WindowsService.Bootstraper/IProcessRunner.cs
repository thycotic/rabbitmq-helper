using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Messaging;

namespace Thycotic.WindowsService.Bootstraper
{
    [ContractClass(typeof(ProcessRunnerContractClass))]
    public interface IProcessRunner
    {
        Process Start(ProcessStartInfo processInfo);
    }

    /// <summary>
    /// Contract for Process Runner.
    /// </summary>
    [ContractClassFor(typeof (IProcessRunner))]
    public abstract class ProcessRunnerContractClass : IProcessRunner
    {
        /// <summary>
        /// Starts the Process Runner.
        /// </summary>
        /// <param name="processInfo">Process info.</param>
        /// <returns></returns>
        public Process Start(ProcessStartInfo processInfo)
        {
            Contract.Requires<ArgumentNullException>(processInfo != null);
            return default(Process);
        }
    }
}