using System;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public class DefaultConfigValues
    {
        public const string Exchange = "thycotic";
        public const string ExchangeType = "topic";
        public const int ReOpenDelay = 10*1000; //10 seconds
        public static readonly TimeSpan ConfirmationTimeout = TimeSpan.FromSeconds(10);

        public class Model
        {
            public const int RetryAttempts = 7;
            public const int RetryDelayMs = 100;
            public const int RetryDelayGrowthFactor = 2;

            public class Publish
            {
                public const bool Mandatory = true;
                public const bool DeliverImmediatelyAndRequireAListener = true;
                public const bool DoNotDeliverImmediatelyOrRequireAListener = false;
                
            }
        }
    }
}
