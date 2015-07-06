using System.Diagnostics.Contracts;

namespace Thycotic.Messages.Common
{
    /// <summary>
    /// Interface for a consumer
    /// </summary>.
    [ContractClass(typeof(ConsumerContract))]
    public interface IConsumer
    {
    }


    /// <summary>
    /// Contract for IConsumer
    /// </summary>
    [ContractClassFor(typeof (IConsumer))]
    public abstract class ConsumerContract : IConsumer
    {
        
    }
}