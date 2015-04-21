namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Blocking consumable base
    /// </summary>
    public abstract class BlockingConsumableBase : IBlockingConsumable
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}