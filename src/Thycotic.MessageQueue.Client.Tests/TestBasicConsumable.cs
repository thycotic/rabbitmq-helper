using System;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Tests
{
    public class TestBasicConsumable : IBasicConsumable
    {
        public int Version { get; private set; }
        public bool Redelivered { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public bool RelayEvenIfExpired { get; set; }
        public string Content { get; set; }
    }
}