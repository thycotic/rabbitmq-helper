using System;
using Thycotic.DistributedEngine.EngineToServerCommunication;
using Thycotic.DistributedEngine.EngineToServerCommunication.Engine.Update;

namespace Thycotic.DistributedEngine.Logic.Update
{
    /// <summary>
    /// Engine to server communication callback
    /// </summary>
    public class EngineToServerCommunicationCallback : IEngineToServerCommunicationCallback
    {

        //public EventHandler<MemoryMqDeliveryEventArgs> BytesReceived;


        /// <summary>
        /// Event fired when bytes are received
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        public void SendUpdateChunk(FileChunk chunk)
        {
            
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
         
        }
    }
}
