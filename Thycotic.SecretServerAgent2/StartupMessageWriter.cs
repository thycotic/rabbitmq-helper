using System;
using Autofac;

namespace Thycotic.SecretServerAgent2
{
    public class StartupMessageWriter : IStartable
    {
        public void Start()
        {
            Console.WriteLine("Application is starting...");
        }
    }
}
