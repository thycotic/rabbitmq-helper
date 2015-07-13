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
        /// <param name="isLegacyAgent">if set to <c>true</c> [is legacy].</param>
        void ApplyLatestUpdate(bool isLegacyAgent = false);
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
        /// <param name="isLegacyAgent">if set to <c>true</c> [is legacy].</param>
        public void ApplyLatestUpdate(bool isLegacyAgent = false)
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
