using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.Messages
{
    public interface IConsumable
    {
        int Version { get; }
        int RetryCount { get; set; }
    }
}
