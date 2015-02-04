using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services")]
    public interface IMemoryMqServerCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(MemoryQueueDeliveryEventArgs deliveryArgs);
    }
}