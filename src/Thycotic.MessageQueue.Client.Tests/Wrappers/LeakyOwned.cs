using System;
using Autofac.Features.OwnedInstances;

namespace Thycotic.MessageQueue.Client.Tests.Wrappers
{
    public class LeakyOwned<T> : Owned<T>
    {
        public LeakyOwned(T value, IDisposable lifetime) : base(value, lifetime)
        {
        }

        protected override void Dispose(bool disposing)
        {
            //don't dispose the value
        }
    }
}