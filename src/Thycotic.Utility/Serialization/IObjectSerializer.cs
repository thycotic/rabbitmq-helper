using System;
using System.Diagnostics.Contracts;

namespace Thycotic.Utility.Serialization
{
    /// <summary>
    /// Interface for a message serializer
    /// </summary>
    [ContractClass(typeof(ObjectSerializerContract))]
    public interface IObjectSerializer
    {
        /// <summary>
        /// Turns the array of bytes into an object of the specified type.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        TRequest ToObject<TRequest>(byte[] bytes);

        /// <summary>
        /// Turns the array of bytes into an object.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        object ToObject(byte[] bytes);

        /// <summary>
        /// Turns the object into an array of bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        byte[] ToBytes(object message);
    }

    /// <summary>
    /// Contract class
    /// </summary>
    [ContractClassFor(typeof(IObjectSerializer))]
    public abstract class ObjectSerializerContract : IObjectSerializer
    {
        /// <summary>
        /// Turns the array of bytes into an object of the specified type.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public TRequest ToObject<TRequest>(byte[] bytes)
        {
            Contract.Requires<ArgumentNullException>(bytes != null);

            return default(TRequest);
        }

        /// <summary>
        /// Turns the array of bytes into an object.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public object ToObject(byte[] bytes)
        {
            Contract.Requires<ArgumentNullException>(bytes != null);

            return default(object);
        }

        /// <summary>
        /// Turns the object into an array of bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public byte[] ToBytes(object message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            return default(byte[]);
        }
    }
}
