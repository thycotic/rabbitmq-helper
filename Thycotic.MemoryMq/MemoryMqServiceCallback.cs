using System;

namespace Thycotic.MemoryMq
{
    public class MemoryMqServiceCallback : IMemoryMqServiceCallback
    {
        public void UpdateStatus(string statusMessage)
        {
            Console.WriteLine(statusMessage);
        }


    }
}