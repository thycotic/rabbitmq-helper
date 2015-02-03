using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services", CallbackContract = typeof(IMemoryMqCallback))]
    public interface IMemoryMqClient
    {
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string meal);

        [OperationContract(IsOneWay = true)]
        void BlockingPublish(string meal);
    }
}