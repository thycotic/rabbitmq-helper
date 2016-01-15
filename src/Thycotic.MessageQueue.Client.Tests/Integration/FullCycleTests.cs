using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.QueueClient;
using Thycotic.MessageQueue.Client.QueueClient.AzureServiceBus;
using Thycotic.MessageQueue.Client.Tests.Wrappers;
using Thycotic.MessageQueue.Client.Wrappers;
using Thycotic.Messages.Common;
using Thycotic.Utility.Serialization;
using Thycotic.Utility.Testing.BDD;
using Thycotic.Utility.Testing.TestChain;

namespace Thycotic.MessageQueue.Client.Tests.Integration
{
    [Ignore]
    public class FullCycleTests :
            BehaviorTestBase<FullFlow>
    {


        [SetUp]
        public override void SetUp()
        {
            Log.Configure();

            Sut = new FullFlow("integration-test");
        }

        /// <summary>
        /// Constructors the parameters do not except invalid parameters.
        /// </summary>
        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            //not needed for integration
        }

        /// <summary>
        /// Basic deliver should not relay corrupted.
        /// </summary>
        [Test]
        [TestCase(1)]
        [TestCase(10)]
        //[TestCase(100)]
        [Ignore("Can't do without consumers")]
        public void ShouldPublish(int messageCount)
        {
            Given(() =>
            {
                //Sut.StartConsuming();

                //Enumerable.Range(0, messageCount)
                //    .AsParallel()
                //    .ForAll(i => Sut.StartFlow(new ResourceGroupMessage { ProvisionId = Guid.NewGuid(), Sequence = i }));

                Enumerable.Range(0, messageCount)
                    .ToList()
                    .ForEach(i => Sut.StartFlow(new ResourceGroupMessage { ProvisionId = new Guid("638d59e9-0ec3-4fe2-8959-1aa7a4b9488f"), Sequence = i }));
            });



            When(() =>
            {

            });


            Then(() =>
            {

            });
        }

        [Test]
        [TestCase(1)]
        //[TestCase(10)]
        //[TestCase(100)]
        public void ShouldPublishAndReceive(int messageCount)
        {
            Given(() =>
            {
                Sut.StartConsuming();

                //Enumerable.Range(0, messageCount)
                //    .AsParallel()
                //    .ForAll(i => Sut.StartFlow(new ResourceGroupMessage { ProvisionId = Guid.NewGuid(), Sequence = i }));

                Enumerable.Range(0, messageCount)
                    .ToList()
                    .ForEach(i => Sut.StartFlow(new ResourceGroupMessage { ProvisionId = new Guid("638d59e9-0ec3-4fe2-8959-1aa7a4b9488f"), Sequence = i }));
            });



            When(() =>
            {

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(360));
                var token = cts.Token;
                while (Sut.CurrentMessageCount < messageCount && !token.IsCancellationRequested)
                {
                    Task.Delay(TimeSpan.FromSeconds(1), token).Wait(token);
                }

            });


            Then(() =>
            {
                Sut.CurrentMessageCount.Should().Be(messageCount);
            });
        }
    }


    #region Helper class


    public class ProvisionConsumable : BasicConsumableBase
    {
        public Guid ProvisionId { get; set; }
        public int Sequence { get; set; }
    }

    public class ResourceGroupMessage : ProvisionConsumable
    {
    }


    public class ServiceBusMessage : ProvisionConsumable
    {

    }

    public class SqlServerMessage : ProvisionConsumable
    {

    }

    public class StorageMessage : ProvisionConsumable
    {

    }


    public class FullFlowConsumer : IBasicConsumer<ResourceGroupMessage>,

        IBasicConsumer<SqlServerMessage>,
        IBasicConsumer<StorageMessage>,
            IBasicConsumer<ServiceBusMessage>
    {
        private static string _exchangeName;
        private static IRequestBus _requestBus;
        private readonly ILogWriter _log = Log.Get(typeof(FullFlowConsumer));

        public FullFlowConsumer(string exchangeName, IRequestBus requestBus)
        {
            _exchangeName = exchangeName;
            _requestBus = requestBus;
        }

        public void Consume(CancellationToken token, ResourceGroupMessage request)
        {
            _log.Debug("Provision Resource Group for : " + request.ProvisionId);
            try
            {
                _log.Debug("Begin Consume Async Resource Group for : " + request.ProvisionId);
                ConsumeAsync(token, request).Wait(CancellationToken.None);
                _log.Debug("Completed Consume Async Resource Group for : " + request.ProvisionId);
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        internal async Task ConsumeAsync(CancellationToken token, ResourceGroupMessage request)
        {
            try
            {
                _log.Debug("Begin Resource Group Logic for : " + request.ProvisionId);
                var result = await Task.FromResult(new LogicResult { Success = true });
                //await _logic.ProvisionResourceGroup(request.ProvisionId).ConfigureAwait(false);
                _log.Debug("Completed Resource Group Logic for : " + request.ProvisionId);

                if (!result.Success)
                {
                    //if (result.Severity == Severity.Retry)
                    //{
                    //    _log.Debug("Will Try Again to Provision Resource Group for : " + request.ProvisionId);
                    //    await Task.Delay(TimeSpan.FromSeconds(1), token).ConfigureAwait(false);
                    //    _requestBus.BasicPublish(new ResourceGroupMessage { ProvisionId = request.ProvisionId });
                    //    _log.Debug("Queued Provision of Resource Group for : " + request.ProvisionId);
                    //    return;
                    //}
                }

                _log.Debug("Begin Basic Publish of Child Messages for : " + request.ProvisionId);
                if (!token.IsCancellationRequested)
                {
                    _log.Debug("Queue Provision of Storage for : " + request.ProvisionId);
                    _requestBus.BasicPublish(_exchangeName, new StorageMessage { ProvisionId = request.ProvisionId, Sequence = request.Sequence });
                    _log.Debug("Queued Provision Storage for : " + request.ProvisionId);

                    _log.Debug("Queue Provision of Sql Server for : " + request.ProvisionId);
                    _requestBus.BasicPublish(_exchangeName, new SqlServerMessage { ProvisionId = request.ProvisionId, Sequence = request.Sequence });
                    _log.Debug("Queued Provision of Sql Server for : " + request.ProvisionId);

                    _log.Debug("Queue Provision of Service Bus for : " + request.ProvisionId);
                    _requestBus.BasicPublish(_exchangeName, new ServiceBusMessage { ProvisionId = request.ProvisionId, Sequence = request.Sequence });
                    _log.Debug("Queued Provision of Service Bus for : " + request.ProvisionId);
                }
                _log.Debug("Completed Basic Publish of Child Messages for : " + request.ProvisionId);
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }

        public void Consume<T>(CancellationToken token, T request) where T : ProvisionConsumable
        {
            _log.Debug("Provision Resource Group for : " + request.ProvisionId);
            try
            {
                _log.Debug("Begin Consume Async Resource Group for : " + request.ProvisionId);
                ConsumeAsync(token, request).Wait(CancellationToken.None);
                _log.Debug("Completed Consume Async Resource Group for : " + request.ProvisionId);
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }
        internal async Task ConsumeAsync<T>(CancellationToken token, T request) where T : ProvisionConsumable
        {
            try
            {
                _log.Debug(string.Format("Begin {0} for {1}/{2}", typeof(T), request.ProvisionId, request.Sequence));

                await Task.FromResult(true);// Task.Delay(TimeSpan.FromHours(100));
                //await _logic.ProvisionResourceGroup(request.ProvisionId).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _log.Error(exception.Message, exception);
            }
        }


        public void Consume(CancellationToken token, StorageMessage request)
        {
            Consume<StorageMessage>(token, request);
        }

        public void Consume(CancellationToken token, SqlServerMessage request)
        {
            Consume<SqlServerMessage>(token, request);
        }


        public void Consume(CancellationToken token, ServiceBusMessage request)
        {
            Consume<ServiceBusMessage>(token, request);
        }
    }



    public class LogicResult
    {
        public bool Success { get; set; }
        public object Severity { get; set; }
    }

    public class CountingBasicConsumerWrapper<TConsumable, TConsumer> : BasicConsumerWrapper<TConsumable, TConsumer>
        where TConsumer : IBasicConsumer<TConsumable>
        where TConsumable : class, IBasicConsumable
    {

        private long _currentCount;

        public long CurrentMessageCount
        {
            get { return Interlocked.Read(ref _currentCount); }
        }


        public CountingBasicConsumerWrapper(ICommonConnection connection, IExchangeNameProvider exchangeNameProvider, IObjectSerializer objectSerializer, IMessageEncryptor messageEncryptor, IPrioritySchedulerProvider prioritySchedulerProvider, Func<Owned<TConsumer>> consumerFactory)
            : base(connection, exchangeNameProvider, objectSerializer, messageEncryptor, prioritySchedulerProvider, consumerFactory)
        {
        }

        public override void PostConsume(CancellationToken token, TConsumable message)
        {
            Interlocked.Increment(ref _currentCount);
        }
    }


    public class FullFlow
    {

        private readonly ICommonConnection _commonConnection;
        private readonly IExchangeNameProvider _exchangeNameProvider;
        private readonly IObjectSerializer _objectSerializer;
        private readonly IMessageEncryptor _messageEncryptor;
        private readonly IPrioritySchedulerProvider _prioritySchedulerProvider;

        private CountingBasicConsumerWrapper<ResourceGroupMessage, FullFlowConsumer> _consumerWrapper1;
        private CountingBasicConsumerWrapper<SqlServerMessage, FullFlowConsumer> _consumerWrapper2;
        private CountingBasicConsumerWrapper<StorageMessage, FullFlowConsumer> _consumerWrapper3;
        private CountingBasicConsumerWrapper<ServiceBusMessage, FullFlowConsumer> _consumerWrapper4;
        private readonly string _exchangeName;
        private readonly RequestBus _requestBus;

        public FullFlow(string exchangeName)
        {
            string connectionString = "sb://bus01-cm01-ss-east-us-thycotic.servicebus.windows.net/";
            string sharedAccessKeyName = "RootManageSharedAccessKey";
            string sharedAccessKeyValue = "L3sKqG1bvI3K+4ue/9m5fVgfI4Wip2drGNX2rbF9bxE=";
            _commonConnection = new AzureServiceBusConnection(connectionString, sharedAccessKeyName,
                sharedAccessKeyValue);

            _exchangeNameProvider = TestedSubstitute.For<IExchangeNameProvider>();
            _exchangeName = exchangeName;//"" "IntegrationTest"; // this.GenerateUniqueDummyName();
            _exchangeNameProvider.GetCurrentExchange().Returns(_exchangeName);



            _objectSerializer = new JsonObjectSerializer();
            _messageEncryptor = TestedSubstitute.For<IMessageEncryptor>();

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Encrypt(Arg.Any<string>(), Arg.Any<byte[]>())
                .Returns(info => info.Args().Skip(1).First());

            //just return what is sent (skip the first argument - exchange - and return the bytes)
            _messageEncryptor.Decrypt(Arg.Any<string>(), Arg.Any<byte[]>())
                .Returns(info => info.Args().Skip(1).First());

            _requestBus = new RequestBus(_commonConnection, _objectSerializer, _messageEncryptor);

            _prioritySchedulerProvider = TestedSubstitute.For<IPrioritySchedulerProvider>();
            var testContext = new SynchronizationContext();
            _prioritySchedulerProvider.Lowest.Returns(new PriorityScheduler(testContext, ThreadPriority.Lowest));
            _prioritySchedulerProvider.BelowNormal.Returns(new PriorityScheduler(testContext,
                ThreadPriority.BelowNormal));
            _prioritySchedulerProvider.Normal.Returns(new PriorityScheduler(testContext, ThreadPriority.Normal));
            _prioritySchedulerProvider.AboveNormal.Returns(new PriorityScheduler(testContext,
                ThreadPriority.AboveNormal));
            _prioritySchedulerProvider.Highest.Returns(new PriorityScheduler(testContext, ThreadPriority.Highest));


            Func<Owned<FullFlowConsumer>> consumerFactory =
                () =>
                    new LeakyOwned<FullFlowConsumer>(new FullFlowConsumer(_exchangeName, _requestBus),
                        new LifetimeDummy());

            _consumerWrapper1 = new CountingBasicConsumerWrapper<ResourceGroupMessage, FullFlowConsumer>(_commonConnection,
               _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider,
               consumerFactory);

            _consumerWrapper2 = new CountingBasicConsumerWrapper<SqlServerMessage, FullFlowConsumer>(_commonConnection,
               _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider,
               consumerFactory);

            _consumerWrapper3 = new CountingBasicConsumerWrapper<StorageMessage, FullFlowConsumer>(_commonConnection,
               _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider,
              consumerFactory);

            _consumerWrapper4 = new CountingBasicConsumerWrapper<ServiceBusMessage, FullFlowConsumer>(_commonConnection,
               _exchangeNameProvider, _objectSerializer, _messageEncryptor, _prioritySchedulerProvider,
               consumerFactory);
        }

        public long CurrentMessageCount
        {
            get
            {
                var counts = new[] { _consumerWrapper1.CurrentMessageCount,
                    _consumerWrapper2.CurrentMessageCount,
                    _consumerWrapper3.CurrentMessageCount,
                    _consumerWrapper4.CurrentMessageCount};

                return counts.Min();


            }
        }

        public void StartFlow(ResourceGroupMessage resourceGroupMessage)
        {
            _requestBus.BasicPublish(_exchangeName, resourceGroupMessage);
        }

        public void StartConsuming()
        {
            _consumerWrapper1.StartConsuming();
            _consumerWrapper2.StartConsuming();
            _consumerWrapper3.StartConsuming();
            _consumerWrapper4.StartConsuming();
        }



    }
    #endregion
}