using System.Text;
using Newtonsoft.Json;

namespace Thycotic.MessageQueueClient
{
    public interface IMessageSerializer
    {
        TRequest BytesToMessage<TRequest>(byte[] bytes);
        byte[] MessageToBytes(object message);
    }

    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public TRequest BytesToMessage<TRequest>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<TRequest>(Encoding.UTF8.GetString(bytes), _serializerSettings);
        }

        public byte[] MessageToBytes(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, Formatting.None, _serializerSettings));
        }
    }
}
