using System;

namespace Thycotic.Messages.Common.Tests
{
    public class BasicConsumerDummy : IBasicConsumer<object>
    {
        public void Consume(object request)
        {
            //contracts should kick in
            if (request == null)
            {
                throw new NotSupportedException("Code contracts did not kick in");
            }
        }
    }
}