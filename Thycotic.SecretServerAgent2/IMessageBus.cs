using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Thycotic.Messages;

namespace Thycotic.SecretServerAgent2
{
    public interface IMessageBus
    {
        void Publish(IConsumable consumable);
    }
}
