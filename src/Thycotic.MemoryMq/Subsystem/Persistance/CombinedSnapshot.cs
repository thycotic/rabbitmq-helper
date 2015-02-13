namespace Thycotic.MemoryMq.Subsystem.Persistance
{
    /// <summary>
    /// Snapshot
    /// </summary>
    public class CombinedSnapshot
    {
        /// <summary>
        /// Gets or sets the exchanges.
        /// </summary>
        /// <value>
        /// The exchanges.
        /// </value>
        public ExchangeSnapshot[] Exchanges { get; set; }
    }
}