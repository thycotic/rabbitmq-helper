using System;

namespace Thycotic.Messages.Common.Tests
{
    internal class BasicConsumerDummy : IBasicConsumer<BasicConsumableDummy>
    {
        public void Consume(BasicConsumableDummy request)
        {
            //contracts should kick in
            if (request == null)
            {
                throw new NotSupportedException("Code contracts did not kick in");
            }
        }
    }
}