using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// PrivateKey
    /// </summary>
    public class PrivateKey : KeyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateKey"/> class.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        public PrivateKey(byte[] privateKey)
            : base(privateKey)
        {
        }
    }
}
