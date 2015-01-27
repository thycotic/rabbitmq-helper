using Thycotic.Logging;
using Thycotic.Messages;

namespace Thycotic.SecretServerAgent2.ServiceBus
{
    public class RabbitMqMessageBus : IMessageBus
    {

        private readonly ILogWriter _log = Log.Get(typeof(RabbitMqMessageBus));
        //TODO: Inject queue client

        public void Publish(IConsumable consumable)
        {
           _log.Debug(string.Format("Publishing {0}", consumable.GetType()));
            //throw new NotImplementedException();
        }
    }
}