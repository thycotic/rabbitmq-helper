using System;

namespace Thycotic.MemoryMq
{
    public class MemoryMqCallback : IMemoryMqCallback
    {
        public void UpdateStatus(string statusMessage)
        {
            Console.WriteLine(statusMessage);
        }


    }
}