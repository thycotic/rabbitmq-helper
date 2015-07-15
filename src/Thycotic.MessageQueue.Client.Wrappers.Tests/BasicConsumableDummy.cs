using System;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    public class BasicConsumableDummy : IBasicConsumable
    {
        public int Version { get; set; }

        public bool Redelivered { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public bool RelayEvenIfExpired { get; set; }

        public string Content { get; set; }
    }
}