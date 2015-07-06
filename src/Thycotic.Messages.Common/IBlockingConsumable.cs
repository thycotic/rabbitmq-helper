using System.Diagnostics.Contracts;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a blocking consumable
    /// </summary>
    [ContractClass(typeof(BlockingConsumableContract))]
    public interface IBlockingConsumable : IConsumable
    {
    }

    /// <summary>
    /// Contract for IBlockingConsumable
    /// </summary>
    [ContractClassFor(typeof(IBlockingConsumable))]
    public abstract class BlockingConsumableContract : IBlockingConsumable
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; private set; }
    }
}