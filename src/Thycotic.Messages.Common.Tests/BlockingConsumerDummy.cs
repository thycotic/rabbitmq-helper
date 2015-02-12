using System;

namespace Thycotic.Messages.Common.Tests
{
    internal class BlockingConsumerDummy : IBlockingConsumer<object, object>
    {
        public object Consume(object request)
        {
            //contracts should kick in
            if (request == null)
            {
                throw new NotSupportedException("Code contracts did not kick in");
            }

            return null;
        }
    }
}