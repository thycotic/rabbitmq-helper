using RabbitMQ.Client;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor);
    }
}
