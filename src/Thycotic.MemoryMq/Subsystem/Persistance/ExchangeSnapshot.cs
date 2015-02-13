namespace Thycotic.MemoryMq.Subsystem.Persistance
{
    /// <summary>
    /// 
    /// </summary>
    public class ExchangeSnapshot
    {
        /// <summary>
        /// Gets or sets the routing slip.
        /// </summary>
        /// <value>
        /// The routing slip.
        /// </value>
        public RoutingSlip RoutingSlip { get; set; }

        /// <summary>
        /// Gets or sets the delivery event arguments.
        /// </summary>
        /// <value>
        /// The delivery event arguments.
        /// </value>
        public MemoryMqDeliveryEventArgs[] DeliveryEventArguments { get; set; }
    }
}