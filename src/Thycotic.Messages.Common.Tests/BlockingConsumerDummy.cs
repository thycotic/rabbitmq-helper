using System;

namespace Thycotic.Messages.Common.Tests
{
    internal class BlockingConsumerDummy : IBlockingConsumer<BlockingConsumableDummy, BlockingConsumerResult>
    {
        public BlockingConsumerResult Consume(BlockingConsumableDummy request)
        {
            //contracts should kick in
            if (request == null)
            {
                throw new NotSupportedException("Code contracts did not kick in");
            }

            return new BlockingConsumerResult {Status = true};
        }
    }
}