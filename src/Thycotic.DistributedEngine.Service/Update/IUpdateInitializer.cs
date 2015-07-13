using System;
using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Service.Update
{
    /// <summary>
    /// Interface for an updated
    /// </summary>
    [ContractClass(typeof(UpdateInitializerContract))]
    public interface IUpdateInitializer : IDisposable
    {
        /// <summary>
        /// Applies the latest update.
        /// </summary>
        void ApplyLatestUpdate();
    }


    /// <summary>
    /// Contract for IUpdateInitializer
    /// </summary>
    [ContractClassFor(typeof(IUpdateInitializer))]
    public abstract class UpdateInitializerContract : IUpdateInitializer
    {
        /// <summary>
        /// Applies the latest update.
        /// </summary>
        public void ApplyLatestUpdate()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}
