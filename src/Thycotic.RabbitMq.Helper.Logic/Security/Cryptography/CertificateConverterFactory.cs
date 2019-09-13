using System;
using System.Security.Cryptography;
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
            try
            {

                if (cert.HasPrivateKey)
                {
                    return new RSACertificateConverter();
                }

                return new BasicCertificateConverter();
            }
            catch (Exception e)
            {
                throw new NotSupportedException("The certificate you are trying to use not supported", e);
            }
        }
    }
}
