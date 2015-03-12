using System;
using System.Security.Cryptography;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    /// <summary>
    /// ByteSaltProvider
    /// </summary>
    public class ByteSaltProvider
    {
        /// <summary>
        /// Salts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="saltLength">Length of the salt.</param>
        /// <returns></returns>
        public byte[] Salt(byte[] data, int saltLength)
        {
            byte[] saltedBytes = new byte[saltLength];
            byte[] saltedData = new byte[data.Length + saltLength];
            RandomNumberGenerator.Create().GetBytes(saltedBytes);
            saltedBytes.CopyTo(saltedData, 0);
            data.CopyTo(saltedData, saltLength);
            return saltedData;
        }

        /// <summary>
        /// Unsalts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="saltLength">Length of the salt.</param>
        /// <returns></returns>
        public byte[] Unsalt(byte[] data, int saltLength)
        {
            byte[] unsaltedData = new byte[data.Length - saltLength];
            Array.ConstrainedCopy(data, saltLength, unsaltedData, 0, unsaltedData.Length);
            return unsaltedData;
        }
    }
}
