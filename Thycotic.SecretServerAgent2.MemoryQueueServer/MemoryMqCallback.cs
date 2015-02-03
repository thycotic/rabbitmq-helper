using System;

namespace Thycotic.SecretServerAgent2.MemoryQueueServer
{
    public class MemoryMqCallback : IMemoryMqCallback
    {
      public void UpdateStatus(string statusMessage)
        {
            Console.WriteLine(statusMessage);
        }

      
    }
}