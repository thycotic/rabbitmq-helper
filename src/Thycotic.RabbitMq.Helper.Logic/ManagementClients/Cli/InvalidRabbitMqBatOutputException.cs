using System;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli
{
    /// <summary>
    /// Exception for output that is not expected from the RabbitMq batch files.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class InvalidRabbitMqBatOutputException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRabbitMqBatOutputException" /> class.
        /// </summary>
        /// <param name="expectedOutput">The exptected output.</param>
        /// <param name="actualOutput">The actual output.</param>
        public InvalidRabbitMqBatOutputException(string expectedOutput, string actualOutput) : base($"Expected \"{expectedOutput}\" but received \"{actualOutput}\"")
        {
        }
    }
}