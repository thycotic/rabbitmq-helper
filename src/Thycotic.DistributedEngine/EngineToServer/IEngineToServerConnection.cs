using System;
using Thycotic.DistributedEngine.Security;
using Thycotic.Encryption;
using Thycotic.Utility.Serialization;

namespace Thycotic.DistributedEngine.EngineToServer
{
    /// <summary>
    /// Interface for an engine to service connection
    /// </summary>
    public interface IEngineToServerConnection : IDisposable
    {
        /// <summary>
        /// Opens the channel.
        /// </summary>
        /// <param name="objectSerializer"></param>
        /// <param name="engineToServerEncryptor"></param>
        /// <returns></returns>
        IEngineToServerChannel OpenChannel(IObjectSerializer objectSerializer, IEngineToServerEncryptor engineToServerEncryptor);

    }
}