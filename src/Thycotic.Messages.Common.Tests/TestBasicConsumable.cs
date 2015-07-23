using System;

namespace Thycotic.Messages.Common.Tests
{
    public class TestBasicConsumable : IBasicConsumable
    {
        public int Version { get; set; }

        public bool Redelivered { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public bool RelayEvenIfExpired { get; set; }

        public string Content { get; set; }
    }
}