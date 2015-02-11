using System.Text;
using Newtonsoft.Json;

namespace Thycotic.MessageQueueClient
{
    /// <summary>
    /// JSON message serializer based on JSON.NET
    /// </summary>
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        /// <summary>
        /// Turns the array of bytes into an object of the specified type.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public TRequest ToRequest<TRequest>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<TRequest>(Encoding.UTF8.GetString(bytes), _serializerSettings);
        }

        /// <summary>
        /// Turns the object into an array of bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public byte[] ToBytes(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, Formatting.None, _serializerSettings));
        }
    }
}