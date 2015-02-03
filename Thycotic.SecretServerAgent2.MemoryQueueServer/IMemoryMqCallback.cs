using System.ServiceModel;

namespace Thycotic.SecretServerAgent2.MemoryQueueServer
{
    [ServiceContract(Namespace = "http://remondo.net/services")]
    public interface IMemoryMqCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateStatus(string statusMessage);
    }
}