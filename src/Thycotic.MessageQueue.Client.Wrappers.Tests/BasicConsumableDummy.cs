using System;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    internal class BasicConsumableDummy : IBasicConsumable
    {
        public BasicConsumableDummy(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }

        public bool Redelivered { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public bool RelayEvenIfExpired { get; set; }
    }

    internal class BlockingConsumableDummy : IBlockingConsumable
    {
        public BlockingConsumableDummy(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
    }
}