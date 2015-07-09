//Feature: BlockingBlockingConsumerWrapper
	

//Background: 
//    Given there exists a substitute object for ICommonConnection stored in the scenario as CommonConnectionTest
//    And there exists a substitute object for IExchangeNameProvider stored in the scenario as ExchangeNameProviderTest
//    And there exists a substitute object for IBlockingConsumer<BlockingConsumableDummy, object> stored in the scenario as BlockingConsumerTest
//    And there exists a blocking consumer factory function stored in the scenario as ConsumerFactoryTest which returns Owned<IBlockingConsumer<BlockingConsumableDummy>> of IBlockingConsumer<BlockingConsumableDummy> BlockingConsumerTest
//    And there exists a substitute object for IObjectSerializer stored in the scenario as ObjectSerializerTest
//    And there exists a substitute object for IMessageEncryptor stored in the scenario as MessageEncryptorTest
//    And there exists a BlockingConsumableDummy stored in the scenario as BlockingConsumableDummyTest
//    And the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns BlockingConsumableDummy BlockingConsumableDummyTest
//    And there exists a BlockingConsumerWrapperDummy stored in the scenario as BlockingConsumerWrapperDummyTest with CommonConnection CommonConnectionTest, ExchangeNameProvider ExchangeNameProviderTest, ConsumerFactory ConsumerFactoryTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest

//Scenario: HandleBlockingDeliver should relay message
//    When the connection is established on ICommonConnection CommonConnectionTest
//    When the method HandleBasicDeliver on BlockingConsumerWrapperDummy BlockingConsumerWrapperDummyTest is called
//    Then the method Consume on IBlockingConsumer<BlockingConsumableDummy> BlockingConsumerTest is called
//    Then the method BlockingAck on the CommonModel of BlockingConsumerWrapperDummy BlockingConsumerWrapperDummyTest is called
//    Then the method OpenChannel on ICommonConnection CommonConnectionTest is called
	
//Scenario: HandleBlockingDeliver should throw away non parsable message
//    Given the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns corrupted BlockingConsumableDummy message
//    When the connection is established on ICommonConnection CommonConnectionTest
//    When the method HandleBasicDeliver on BlockingConsumerWrapperDummy BlockingConsumerWrapperDummyTest is called
//    Then the method Consume on IBlockingConsumer<BlockingConsumableDummy> BlockingConsumerTest is not called
//    Then the method BlockingNack on the CommonModel of BlockingConsumerWrapperDummy BlockingConsumerWrapperDummyTest is called
	



using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{


    internal class BlockingConsumerWrapperDummy : BlockingConsumerWrapper<BlockingConsumableDummy, object, IBlockingConsumer<BlockingConsumableDummy, object>>
    {
        public Task HandleTask { get; set; }

        public BlockingConsumerWrapperDummy(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer,
            IMessageEncryptor messageEncryptor, Func<Owned<IBlockingConsumer<BlockingConsumableDummy, object>>> consumerFactory)
            : base(connection, exchangeNameProvider, objectSerializer, messageEncryptor, consumerFactory)
        {

        }

        protected override Task StartHandleTask(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
           ICommonModelProperties properties, byte[] body)
        {
            var task = base.StartHandleTask(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
            HandleTask = task;
            return task;
        }


    }
}
