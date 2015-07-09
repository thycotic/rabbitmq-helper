using System;
using System.Diagnostics.Contracts;
using System.Text;
using Newtonsoft.Json;

namespace Thycotic.Utility.Serialization
{
    /// <summary>
    /// JSON message serializer based on JSON.NET
    /// </summary>
    public class JsonObjectSerializer : IObjectSerializer
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        /// <summary>
        /// Turns the array of bytes into an object of the specified type.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public TRequest ToObject<TRequest>(byte[] bytes)
        {
            //TODO: Blow up if you can't deserialize!!!!
            var str = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<TRequest>(str, _serializerSettings);
        }

        /// <summary>
        /// Turns the array of bytes into an object.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public object ToObject(byte[] bytes)
        {
            //TODO: Blow up if you can't deserialize!!!!
            var str = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<object>(str, _serializerSettings);
        }

        /// <summary>
        /// Turns the object into an array of bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public byte[] ToBytes(object message)
        {
            var serialized = JsonConvert.SerializeObject(message, Formatting.None, _serializerSettings);

            if (string.IsNullOrWhiteSpace(serialized))
            {
                throw new ApplicationException("Could not serialize");
            }

            Contract.Assume(serialized != null);
            Contract.Assume(!string.IsNullOrWhiteSpace(serialized));

            return Encoding.UTF8.GetBytes(serialized);
        }
    }
}