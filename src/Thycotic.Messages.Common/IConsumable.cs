using System.Diagnostics.Contracts;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a consumable
    /// </summary>
    [ContractClass(typeof(ConsumableContract))]
    public interface IConsumable
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; }
    }

    /// <summary>
    /// Contract for IConsumable
    /// </summary>
    [ContractClassFor(typeof(IConsumable))]
    public abstract class ConsumableContract : IConsumable
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
