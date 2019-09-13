using System.Security.Cryptography.X509Certificates;

namespace Thycotic.RabbitMq.Helper.Logic.Security.Cryptography
{
    /// <summary>
    /// Interface for a certificate converter
    /// </summary>
    public interface ICertificateConverter
    {
        /// <summary>
        /// Saves the certificate to pem.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="path">The path.</param>
        void SaveCertificateToPem(X509Certificate2 cert, string path);

        /// <summary>
        /// Saves the private key to pem key.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="path">The path.</param>
        void SavePrivateKeyToPemKey(X509Certificate2 cert, string path);
    }
}