using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Thycotic.RabbitMq.Helper.Logic.Security.Cryptography
{
    /// <summary>
    /// Basic certificate converted that does not support private key usage.
    /// </summary>
    /// <seealso cref="Thycotic.RabbitMq.Helper.Logic.Security.Cryptography.ICertificateConverter" />
    public class BasicCertificateConverter : ICertificateConverter
    {
        /// <inheritdoc />
        public void SaveCertificateToPem(X509Certificate2 cert, string path)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(cert));
                    streamWriter.Flush();

                    File.WriteAllBytes(path, memoryStream.GetBuffer());
                }
            }
        }

        /// <inheritdoc />
        public void SavePrivateKeyToPemKey(X509Certificate2 cert, string path)
        {
            throw new NotSupportedException();
        }
    }
}