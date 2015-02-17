namespace Thycotic.Utility.Serialization
{
    /// <summary>
    /// Interface for a message serializer
    /// </summary>
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
        /// Turns the object into an array of bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        byte[] ToBytes(object message);
    }
}
