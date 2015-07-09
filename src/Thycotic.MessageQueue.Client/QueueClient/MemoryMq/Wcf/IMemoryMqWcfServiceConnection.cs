using System;
using System.Diagnostics.Contracts;
using Thycotic.Wcf;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf
{
    [ContractClass(typeof(MemoryMqWcfServiceConnectionContract))]
    internal interface IMemoryMqWcfServiceConnection : IDuplexWcfConnection
    {
        ICommonModel CreateModel();
    }

    /// <summary>
    /// Contract for ICommonConnection
    /// </summary>
    [ContractClassFor(typeof(IMemoryMqWcfServiceConnection))]
    public abstract class MemoryMqWcfServiceConnectionContract : IMemoryMqWcfServiceConnection
    {
        public bool IsOpen { get; set; }

        public EventHandler ConnectionShutdown { get; set; }

        public ICommonModel CreateModel()
        {
            Contract.Ensures(Contract.Result<ICommonModel>() != null);

            return default(ICommonModel);
        }


        public void Close(int timeoutMilliseconds)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(timeoutMilliseconds >= 0);
        }
        
        public void Dispose()
        {
        }

    }
}