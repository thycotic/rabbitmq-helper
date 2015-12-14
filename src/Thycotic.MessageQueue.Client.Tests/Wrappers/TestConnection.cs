using System;
using Thycotic.MessageQueue.Client.QueueClient;

namespace Thycotic.MessageQueue.Client.Tests.Wrappers
{
    public class TestConnection : ICommonConnection
    {
        private readonly ICommonModel _model;

        public TestConnection(ICommonModel model)
        {
            _model = model;
        }

        public EventHandler ConnectionCreated { get; set; }

        public string ServerVersion
        {
            get { return GetType().Assembly.GetName().Version.ToString(); }
        }

        public ICommonModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor)
        {
            return _model;
        }

        public void Dispose()
        {

        }

    }
}