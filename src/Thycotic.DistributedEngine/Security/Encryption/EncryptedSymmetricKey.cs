using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// EncryptedSymmetricKey
    /// </summary>
    public class EncryptedSymmetricKey : KeyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedSymmetricKey"/> class.
        /// </summary>
        /// <param name="encryptedKey">The encrypted key.</param>
        public EncryptedSymmetricKey(byte[] encryptedKey)
            : base(encryptedKey)
        {
        }
    }
}
