using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.CLI;
using Thycotic.CLI.Commands;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Installation;

namespace Thycotic.RabbitMq.Helper.Certificate
{
    public class ConvertCaCerToPemCommand : CommandBase, IImmediateCommand
    {
        public static readonly string CertificatePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
            "ca.pem");

        private readonly ILogWriter _log = Log.Get(typeof(ConvertPfxToPemCommand));

        public override string Name
        {
            get { return "convertCaCertToPem"; }
        }

        public override string Area {
            get { return "Certificate"; }
        }

        public override string Description
        {
            get { return "Converts a Certificate Authority cert to a pem."; }
        }

        public ConvertCaCerToPemCommand()
        {

            Action = parameters =>
            {
                string path;
                string password;
                if (!parameters.TryGet("cacertpath", out path)) return 1;
                
                ConvertToPem(path);

                return 0;

            };
        }

        private void ConvertToPem(string cacertpath)
        {

            var file = new FileInfo(cacertpath);

            if (file.Extension.ToLower() != ".cer")
            {
                throw new ApplicationException("File is not .CER");
            }


            if (file.Directory == null || !file.Directory.Exists || !file.Exists)
            {
                throw new ApplicationException("File does not exist");
            }

            X509Certificate2 cert;

            try
            {
                cert = new X509Certificate2(cacertpath);
            }
            catch (Exception ex)
            {
                throw new CertificateException("Could not open CER", ex);
            }

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    _log.Info("Creating certificate file..");

                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(cert));
                    streamWriter.Flush();

                    File.WriteAllBytes(CertificatePath, memoryStream.GetBuffer());

                    _log.Info(string.Format("Certificate file written to {0}", CertificatePath));
                }
            }

            _log.Info("Done");
        }

        
    }
}