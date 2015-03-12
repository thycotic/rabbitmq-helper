using System;
using System.Security.Cryptography;

namespace Thycotic.DistributedEngine.Security.Encryption
{
    class ByteSaltProvider
    {
        public byte[] Salt(byte[] data, int saltLength)
        {
            byte[] saltedBytes = new byte[saltLength];
            byte[] saltedData = new byte[data.Length + saltLength];
            RandomNumberGenerator.Create().GetBytes(saltedBytes);
            saltedBytes.CopyTo(saltedData, 0);
            data.CopyTo(saltedData, saltLength);
            return saltedData;
        }

        public byte[] Unsalt(byte[] data, int saltLength)
        {
            byte[] unsaltedData = new byte[data.Length - saltLength];
            Array.ConstrainedCopy(data, saltLength, unsaltedData, 0, unsaltedData.Length);
            return unsaltedData;
        }
    }
}
