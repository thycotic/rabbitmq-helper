using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client.Wrappers
{
    /// <summary>
    /// Delivery tag wrapper which allows for any kind of delivery tag. Currently supporting ulong and Guid.
    /// </summary>
    public class DeliveryTagWrapper
    {
        /// <summary>
        /// Gets or sets the value`1.
        /// </summary>
        /// <value>
        /// The value`1.
        /// </value>
        public object Value { get; private set; }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryTagWrapper"/> class.
        /// </summary>
        /// <param name="rawValue">The raw value.</param>
        public DeliveryTagWrapper(object rawValue)
        {
            Value = rawValue;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DeliveryTagWrapper"/> to <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ulong(DeliveryTagWrapper value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return (ulong)value.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DeliveryTagWrapper"/> to <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Guid(DeliveryTagWrapper value)
        {
            Contract.Requires<ArgumentNullException>(value != null);

            return (Guid)value.Value;
        }
    }
}
