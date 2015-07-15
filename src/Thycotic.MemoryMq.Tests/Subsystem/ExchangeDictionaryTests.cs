using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Thycotic.MemoryMq.Subsystem;
using Thycotic.Utility.Reflection;
using Thycotic.Utility.Testing.BDD;
using Thycotic.Utility.Testing.DataGeneration;

namespace Thycotic.MemoryMq.Tests.Subsystem
{
    [TestFixture]
    public class ExchangeDictionaryTests : BehaviorTestBase<ExchangeDictionary>
    {
        private readonly AssemblyEntryPointProvider _assemblyEntryPointProvider = new AssemblyEntryPointProvider();
        private string _persistPath;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _persistPath = Path.Combine(_assemblyEntryPointProvider.GetAssemblyDirectory(typeof(ExchangeDictionary)),
                ExchangeDictionary.DataDirectoryName, "store.json");
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            //clean up any persisted data
            if (File.Exists(_persistPath))
            {
                File.Delete(_persistPath);
            }
        }


        [SetUp]
        public override void SetUp()
        {
            Sut = new ExchangeDictionary();
        }

        [Test]
        public override void ConstructorParametersDoNotExceptInvalidParameters()
        {
            //nothing in the constructor
        }

        /// <summary>
        /// Exchanges the should not be empty when message is pushed.
        /// </summary>
        [Test]
        public void ExchangeShouldNotBeEmptyWhenMessageIsPushed()
        {
            Given(() =>
            {
               //nothing 
            });

            When(() =>
            {
                var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
                Sut.Publish(routingSlip, new MemoryMqDeliveryEventArgs());
            });

            Then(() =>
            {
                Sut.IsEmpty.Should().BeFalse();
            });
        }

        /// <summary>
        /// Shoulds the be able to push to multiple mailboxes.
        /// </summary>
        [Test]
        public void ShouldBeAbleToPushToMultipleMailboxes()
        {
            Given(() =>
            {
                //nothing 
            });

            var routingSlip1 = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
            var eventArgs1 = new MemoryMqDeliveryEventArgs();
            var routingSlip2 = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
            var eventArgs2 = new MemoryMqDeliveryEventArgs();
            var routingSlip3= new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
            var eventArgs3 = new MemoryMqDeliveryEventArgs();

            var routingSlip4 = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());

            When(() =>
            {
                Sut.Publish(routingSlip1, eventArgs1);
                Sut.Publish(routingSlip2, eventArgs2);
                Sut.Publish(routingSlip3, eventArgs3);
            });

            Then(() =>
            {
                Sut.Mailboxes.Count.Should().Be(3);
                Sut.Mailboxes.Select(m => m.RoutingSlip).Contains(routingSlip1).Should().BeTrue();
                Sut.Mailboxes.Select(m => m.RoutingSlip).Contains(routingSlip2).Should().BeTrue();
                Sut.Mailboxes.Select(m => m.RoutingSlip).Contains(routingSlip3).Should().BeTrue();

                Sut.Mailboxes.Select(m => m.RoutingSlip).Contains(routingSlip4).Should().BeFalse();

            });
        }

        /// <summary>
        /// Should the not be empty when pulling message without ack.
        /// </summary>
        [Test]
        public void ShouldNotBeEmptyWhenPullingMessageWithoutAck()
        {
            Given(() =>
            {
                var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
                var eventArgs = new MemoryMqDeliveryEventArgs();

                Sut.Publish(routingSlip, eventArgs);
            });


            When(() =>
            {
                MemoryMqDeliveryEventArgs eventArgs;
                Sut.Mailboxes.First().Queue.TryDequeue(out eventArgs);
            });

            Then(() =>
            {
                Sut.Mailboxes.Count.Should().Be(1);
                Sut.IsEmpty.Should().BeFalse();

            });
        }

        /// <summary>
        /// Should be empty when acknowledging a message.
        /// </summary>
        [Test]
        public void ShouldBeEmptyWhenAcknowledgingAMessage()
        {
            var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());

            Given(() =>
            {
                var eventArgs = new MemoryMqDeliveryEventArgs();

                Sut.Publish(routingSlip, eventArgs);
            });


            When(() =>
            {
                MemoryMqDeliveryEventArgs eventArgs;

                //pull
                Sut.Mailboxes.First().Queue.TryDequeue(out eventArgs);

                //ack
                Sut.Acknowledge(eventArgs.DeliveryTag, routingSlip);


            });

            Then(() =>
            {
                //should still have the mailbox
                Sut.Mailboxes.Count.Should().Be(1);

                //but there should not be any messages
                Sut.IsEmpty.Should().BeTrue();

            });
        }

        /// <summary>
        /// Should not be empty when negatively acknowledging a message.
        /// </summary>
        [Test]
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void ShouldNotBeEmptyWhenNegativelyAcknowledgingAMessage(bool requeue, bool empty)
        {
            var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());

            Given(() =>
            {
                var eventArgs = new MemoryMqDeliveryEventArgs();
                
                Sut.Publish(routingSlip, eventArgs);
            });


            When(() =>
            {
                MemoryMqDeliveryEventArgs eventArgs;

                //pull
                Sut.Mailboxes.First().Queue.TryDequeue(out eventArgs);

                //nack
                Sut.NegativelyAcknowledge(eventArgs.DeliveryTag, routingSlip, requeue);
            });

            Then(() =>
            {
                Sut.Mailboxes.Count.Should().Be(1);

                Sut.IsEmpty.Should().Be(empty);

            });
        }

        /// <summary>
        /// Should persist messages to disk.
        /// </summary>
        [Test]
        public void ShouldPersistMessagesToDisk()
        {
            Given(() =>
            {
                //post a few messages
                Enumerable.Range(0,5).ToList().ForEach(i =>
                {
                    var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
                    var eventArgs = new MemoryMqDeliveryEventArgs();

                    Sut.Publish(routingSlip, eventArgs);
                });
            });


            When(() =>
            {
                Sut.PersistMessages();
            });

            Then(() =>
            {
                File.Exists(_persistPath).Should().BeTrue();

                Sut.IsEmpty.Should().BeTrue();

            });
        }


        /// <summary>
        /// Should restore messages from disk.
        /// </summary>
        [Test]
        public void ShouldRestoreMessagesFromDisk()
        {
            var messages = new List<Tuple<RoutingSlip, MemoryMqDeliveryEventArgs>>();

            Given(() =>
            {
                

                //post a few messages
                Enumerable.Range(0, 5).ToList().ForEach(i =>
                {
                    var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
                    var eventArgs = new MemoryMqDeliveryEventArgs
                    {
                        Body = Encoding.UTF8.GetBytes(this.GenerateUniqueDummyName())
                    };

                    messages.Add(new Tuple<RoutingSlip, MemoryMqDeliveryEventArgs>(routingSlip, eventArgs));

                    Sut.Publish(routingSlip, eventArgs);
                });
            });


            When(() =>
            {
                Sut.PersistMessages();

                Sut.IsEmpty.Should().BeTrue();

                Sut.RestorePersistedMessages();
            });

            Then(() =>
            {
                File.Exists(_persistPath).Should().BeFalse();

                Sut.IsEmpty.Should().BeFalse();

                Sut.Mailboxes.Count.Should().Be(messages.Count);

                var routingKeys = Sut.Mailboxes.Select(m => m.RoutingSlip.ToString());

                messages.All(msg => routingKeys.Contains(msg.Item1.ToString())).Should().BeTrue();
            });
        }

        /// <summary>
        /// Should persist messages on dispose.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <param name="exists">if set to <c>true</c> [exists].</param>
        [Test]
        [TestCase(5, true)]
        [TestCase(0, false)]
        public void ShouldPersistMessagesOnDispose(int messageCount, bool exists)
        {
            Given(() =>
            {
                //post a few messages
                Enumerable.Range(0, messageCount).ToList().ForEach(i =>
                {
                    var routingSlip = new RoutingSlip(this.GenerateUniqueDummyName(), this.GenerateUniqueDummyName());
                    var eventArgs = new MemoryMqDeliveryEventArgs
                    {
                        Body = Encoding.UTF8.GetBytes(this.GenerateUniqueDummyName())
                    };

                    Sut.Publish(routingSlip, eventArgs);
                });
            });


            When(() =>
            {
                Sut.Dispose();
            });

            Then(() =>
            {
                File.Exists(_persistPath).Should().Be(exists);
            });
        }
    }
}