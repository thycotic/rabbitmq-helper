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
        void GetUpdate(string path);
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
        public void GetUpdate(string path)
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

