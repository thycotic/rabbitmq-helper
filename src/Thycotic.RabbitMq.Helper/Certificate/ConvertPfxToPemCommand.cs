using System;
using System.IO;
using System.Security.Cryptography;
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
    public class ConvertPfxToPemCommand : CommandBase, IImmediateCommand
    {
        public static readonly string CertificatePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
            "cert.pem");

        public static readonly string KeyPath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
            "cert.key");

        private readonly ILogWriter _log = Log.Get(typeof(ConvertPfxToPemCommand));

        public override string Name
        {
            get { return "convertPftToPem"; }
        }

        public override string Area {
            get { return "Certificate"; }
        }

        public override string Description
        {
            get { return "Converts a PFX cert to a pem/key combination."; }
        }

        public ConvertPfxToPemCommand()
        {

            Action = parameters =>
            {
                string path;
                string password;
                if (!parameters.TryGet("pfxPath", out path)) return 1;
                if (!parameters.TryGet("pfxPw", out password)) return 1;
                
                ConvertToPem(path, password);

                return 0;

            };
        }

        private void ConvertToPem(string pfxPath, string password)
        {

            var file = new FileInfo(pfxPath);

            if (file.Extension.ToLower() != ".pfx")
            {
                throw new ApplicationException("File is not .PFX");
            }


            if (file.Directory == null || !file.Directory.Exists || !file.Exists)
            {
                throw new ApplicationException("File does not exist");
            }

            X509Certificate2 cert;

            try
            {
                cert = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
            catch (Exception ex)
            {
                throw new CertificateException("Could not open PFX. Perhaps the password is wrong", ex);
            }

            var rsa = (RSACryptoServiceProvider)cert.PrivateKey;

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    _log.Info("Creating key file..");

                    var pemWriter = new PemWriter(streamWriter);
                    var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();

           
                    File.WriteAllBytes(KeyPath, memoryStream.GetBuffer());

                    _log.Info(string.Format("Key file written to {0}", KeyPath));
                }
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