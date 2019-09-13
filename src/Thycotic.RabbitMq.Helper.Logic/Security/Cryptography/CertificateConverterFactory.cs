using System;
using System.Security.Cryptography.X509Certificates;

namespace Thycotic.RabbitMq.Helper.Logic.Security.Cryptography
{
    /// <summary>
    /// Certificate converted factory
    /// </summary>
    public class CertificateConverterFactory
    {
        /// <summary>
        /// Gets the converter.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static ICertificateConverter GetConverter(X509Certificate2 cert)
        {
            return new RSACertificateConverter();
        }
    }
}
