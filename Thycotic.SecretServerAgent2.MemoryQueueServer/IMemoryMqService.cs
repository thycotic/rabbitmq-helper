using System.ServiceModel;

namespace Thycotic.SecretServerAgent2.MemoryQueueServer
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services", CallbackContract = typeof(IMemoryMqCallback))]
    public interface IMemoryMqService
    {
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string meal);

        [OperationContract(IsOneWay = true)]
        void BlockingPublish(string meal);
    }
}