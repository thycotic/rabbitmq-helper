using System;
using System.Diagnostics.Contracts;

namespace Thycotic.DistributedEngine.Logic.EngineToServer
{
    /// <summary>
    /// Interface for a Update bus
    /// </summary>
    [ContractClass(typeof(UpdateBusContract))]
    public interface IUpdateBus : IDisposable
    {
        /// <summary>
        /// Gets the update.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="isLegacyAgent">if set to <c>true</c> [is legacy agent].</param>
        void GetUpdate(string path, bool isLegacyAgent = false);
    }


    /// <summary>
    /// Contract
    /// </summary>
    [ContractClassFor(typeof(IUpdateBus))]
    public abstract class UpdateBusContract : IUpdateBus
    {
        /// <summary>
        /// Gets the update.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="isLegacyAgent"></param>
        public void GetUpdate(string path, bool isLegacyAgent = false)
        {
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

    }
}

