using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Interface for an object that has a raw value
    /// </summary>
    [ContractClass(typeof(HasRawValueContract))]
    public interface IHasRawValue
    {
        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        object RawValue { get;  }
    }

    /// <summary>
    /// Contract for IHasRawValue
    /// </summary>
    [ContractClassFor(typeof(IHasRawValue))]
    public abstract class HasRawValueContract : IHasRawValue
    {
        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue { get; private set; }
    }
}