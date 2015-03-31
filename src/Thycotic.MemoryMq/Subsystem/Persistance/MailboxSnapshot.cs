namespace Thycotic.MemoryMq.Subsystem.Persistance
{
    /// <summary>
    /// 
    /// </summary>
    public class MailboxSnapshot
    {
        /// <summary>
        /// Gets or sets the delivery event arguments.
        /// </summary>
        /// <value>
        /// The delivery event arguments.
        /// </value>
        public MemoryMqDeliveryEventArgs[] DeliveryEventArguments { get; set; }
    }
}