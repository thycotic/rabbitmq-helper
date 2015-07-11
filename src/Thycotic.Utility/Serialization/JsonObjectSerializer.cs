using System;
using System.Diagnostics.Contracts;
using System.Text;
using Newtonsoft.Json;
using Thycotic.Utility.MixedContracts;

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
        /// <typeparam name="TObject">The type of the request.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public TObject ToObject<TObject>(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);

            //TODO: Blow up if you can't deserialize!!!!
            return this.EnsureNotNull(JsonConvert.DeserializeObject<TObject>(str, _serializerSettings), "Failed to deserialize");
        }

        /// <summary>
        /// Turns the array of bytes into an object.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public object ToObject(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);

            //TODO: Blow up if you can't deserialize!!!!
            return this.EnsureNotNull(JsonConvert.DeserializeObject<object>(str, _serializerSettings), "Failed to deserialize");
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