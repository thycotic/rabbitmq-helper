using System;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    /// <summary>
    /// Default configuration values for the system
    /// </summary>
    public class DefaultConfigValues
    {
        /// <summary>
        /// Alias for the exchange
        /// </summary>
        public const string Exchange = "thycotic";

        /// <summary>
        /// Alias for the exchange type
        /// </summary>
        public const string ExchangeType = "topic";

        /// <summary>
        /// Alias for the re-open delay
        /// </summary>
        public const int ReOpenDelay = 10*1000; //10 seconds

        /// <summary>
        /// Alias for the confirmation timeout
        /// </summary>
        public static readonly TimeSpan ConfirmationTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Model/Connection specific defaults
        /// </summary>
        public class Model
        {
            /// <summary>
            /// Alias for the retry attempts
            /// </summary>
            public const int RetryAttempts = 7;

            /// <summary>
            /// Alias for the retry delay in milliseconds
            /// </summary>
            public const int RetryDelayMs = 100;

            /// <summary>
            /// Alias for the retry delay growth factor
            /// </summary>
            public const int RetryDelayGrowthFactor = 2;

            /// <summary>
            /// BasicPublish specific defaults
            /// </summary>
            public class Publish
            {
                /// <summary>
                /// Alias for "not mandatory"
                /// </summary>
                public const bool NotMandatory = false;

                /// <summary>
                /// Alias for "mandatory"
                /// </summary>
                public const bool Mandatory = true;

                //public const bool DeliverImmediatelyAndRequireAListener = true;//deprecated in AMQP

                /// <summary>
                /// Alias for "do not deliver immediately or require a listener"
                /// </summary>
                public const bool DoNotDeliverImmediatelyOrRequireAListener = false;
                
            }
        }
    }
}
