using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services")]
    public interface IMemoryMqCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateStatus(string statusMessage);
    }
}