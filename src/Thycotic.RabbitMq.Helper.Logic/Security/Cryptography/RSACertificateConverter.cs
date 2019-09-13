using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Thycotic.RabbitMq.Helper.Logic.Security.Cryptography
{
    /// <summary>
    /// RSA certificate converter
    /// </summary>
    public class RSACertificateConverter : ICertificateConverter
    {
        /// <inheritdoc />
        public void SavePrivateKeyToPemKey(X509Certificate2 cert, string path)
        {
            var rsa = (RSACryptoServiceProvider)cert.PrivateKey;

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var pemWriter = new PemWriter(streamWriter);
                    var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();


                    File.WriteAllBytes(path, memoryStream.GetBuffer());

                }
            }

           
        }


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

    }
}
