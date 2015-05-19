using System;

namespace Thycotic.DistributedEngine.Service.Update
{
    /// <summary>
    /// Interface for an updated
    /// </summary>
    public interface IUpdateInitializer : IDisposable
    {
        /// <summary>
        /// Applies the latest update.
        /// </summary>
        void ApplyLatestUpdate();
    }
}
