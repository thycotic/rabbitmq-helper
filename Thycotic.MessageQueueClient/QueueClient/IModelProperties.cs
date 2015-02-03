using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.MessageQueueClient.QueueClient
{
    /// <summary>
    /// Inte3rface for model properties
    /// </summary>
    public interface IModelProperties
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
        /// Sets the persistent.
        /// </summary>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        void SetPersistent(bool persistent);
    }
}
