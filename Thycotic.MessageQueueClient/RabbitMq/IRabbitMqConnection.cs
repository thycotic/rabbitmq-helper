using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor);
    }
}
