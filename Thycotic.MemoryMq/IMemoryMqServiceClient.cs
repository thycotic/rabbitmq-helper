using System;
using System.ServiceModel;

namespace Thycotic.MemoryMq
{
    [ServiceContract(Namespace = "http://www.thycotic.com/services", CallbackContract = typeof(IMemoryMqServiceCallback))]
    public interface IMemoryMqServiceClient
    {
        [OperationContract(IsOneWay = true)]
        void BasicPublish(string exchangeName, string routingKey, bool mandatory, bool immediate, byte[] body);

        [OperationContract(IsOneWay = true)]
        void BasicNack(ulong deliveryTag, bool multiple);

        [OperationContract(IsOneWay = true)]
        void BasicAck(ulong deliveryTag, bool multiple);



    }
}