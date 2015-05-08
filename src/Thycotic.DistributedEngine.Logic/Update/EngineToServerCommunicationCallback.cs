using System;
using System.Collections.Generic;
using System.Linq;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Update;

namespace Thycotic.DistributedEngine.Logic.Update
{
    /// <summary>
    /// Engine to server communication callback
    /// </summary>
    public class EngineToServerCommunicationCallback : IEngineToServerCommunicationCallback
    {
        private readonly List<FileChunk> _chunks = new List<FileChunk>();

        /// <summary>
        /// Event fired when bytes are received
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        public void SendUpdateChunk(FileChunk chunk)
        {
            lock (_chunks)
            {
                _chunks.Add(chunk);
            }
        }

        /// <summary>
        /// Extracts the update.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FileChunk> ExtractUpdate()
        {
            lock (_chunks)
            {
                if (!_chunks.Any() || _chunks.Count() != _chunks.First().TotalChunks)
                {
                    throw new ApplicationException("Update is incomplete");
                }
                
                var orderedChunks = _chunks.OrderBy(c => c.Index).ToArray();

                _chunks.Clear();

                return orderedChunks;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
         
        }
    }
}
