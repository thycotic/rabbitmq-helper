using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services", CallbackContract = typeof(IMemoryMqServiceCallback))]
    public interface IMemoryMqServiceClient
    {
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string meal);

        [OperationContract(IsOneWay = true)]
        void BlockingPublish(string meal);
    }
}