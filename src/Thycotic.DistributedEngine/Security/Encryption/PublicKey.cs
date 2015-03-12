using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// PublicKey
    /// </summary>
    public class PublicKey : KeyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicKey"/> class.
        /// </summary>
        /// <param name="keyBase">The key base.</param>
        public PublicKey(byte[] keyBase)
            : base(keyBase)
        {
        }
    }
}
