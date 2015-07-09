using System;
using System.Diagnostics.Contracts;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Raw value extensions
    /// </summary>
    public static class RawValueExtensions
    {
        /// <summary>
        /// Gets the raw value in the type requested. Throws is type mismatch
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T GetRawValue<T>(this IHasRawValue obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(obj.RawValue != null);

            Contract.Ensures(Contract.Result<T>() != null);

            return (T) obj.RawValue;
        }

    }
}