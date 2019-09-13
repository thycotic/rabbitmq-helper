using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.RabbitMq.Helper.Logic.Security.Cryptography;

namespace Thycotic.RabbitMq.Helper.UnitTests
{
    [TestFixture]
    public class CertificateTests
    {


        [TestCase(@"M:\development\vso\DistributedEngine\Thycotic.RabbitMq.Helper\src\Thycotic.RabbitMq.Helper.PSCommands\Examples\localhost.pfx", "password1", true)]
        [TestCase(@"C:\Users\dkolev\Downloads\rabbit-ecc.thycotic.space.pfx", "Yagni12#", false)]
        [TestCase(@"C:\Users\dkolev\Downloads\rabbit-cng.thycotic.space.pfx", "Yagni12#", false)]
        public void ShouldConvertPfxToPem(string pfxPath, string password, bool savePrivateKey)
        {
            X509Certificate2 cert;

            try
            {
                cert = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
            catch (Exception ex)
            {
                throw new CertificateException("Could not open PFX. Perhaps the password is wrong", ex);
            }

            
            var tempKeyPath = GetTempPath(".key");
            var tempCertPath = GetTempPath(".pem");

            try
            {

                var converter = CertificateConverterFactory.GetConverter(cert);

                converter.SaveCertificateToPem(cert, tempCertPath);

                if (savePrivateKey)
                {
                    converter.SavePrivateKeyToPemKey(cert, tempKeyPath);
                }
            }
            finally
            {
                if (File.Exists(tempKeyPath))
                {
                    File.Delete(tempKeyPath);
                }

                if (File.Exists(tempCertPath))
                {
                    File.Delete(tempCertPath);
                }
            }
        }

        private string GetTempPath(string extension = null)
        {
            return Path.Combine(Path.GetTempPath(), $"{Path.GetTempFileName()}{extension}");
        }
    }
}
