using System.Diagnostics.Contracts;
using Thycotic.Utility.TestChain;

namespace Thycotic.MessageQueue.Client.QueueClient
{
    /// <summary>
    /// Inte3rface for model properties
    /// </summary>
    [UnitTestsRequired]
    [ContractClass(typeof(CommonModelPropertiesContract))]
    public interface ICommonModelProperties : IHasRawValue
    {
        /// <summary>
        /// Gets if there is a ReplyTo in the properties
        /// </summary>
        /// <returns></returns>
        bool IsReplyToPresent();

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        string ReplyTo { get; set; }


        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        string Type { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICommonModelProperties"/> is persistent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        bool Persistent { get; set; }
    }

    /// <summary>
    /// Contract for CommonModelPropertiesContract 
    /// </summary>
    [ContractClassFor(typeof (ICommonModelProperties))]
    public abstract class CommonModelPropertiesContract : ICommonModelProperties
    {
        /// <summary>
        /// Gets if there is a ReplyTo in the properties
        /// </summary>
        /// <returns></returns>
        public bool IsReplyToPresent()
        {
            return default(bool);
        }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        public string ReplyTo { get; set; }


        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICommonModelProperties" /> is persistent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        public bool Persistent { get; set; }

        /// <summary>
        /// Gets the raw value.
        /// </summary>
        /// <value>
        /// The raw value.
        /// </value>
        public object RawValue { get; private set; }
    }
}
