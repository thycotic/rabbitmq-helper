using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services")]
    public interface IMemoryMqServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(byte[] body);
    }
}