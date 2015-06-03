﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.CLI;
using Thycotic.Logging;

namespace Thycotic.RabbitMq.Helper
{

    class InstallRabbitMqCommand : ConsoleCommandBase, IImmediateConsoleCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Name
        {
            get { return "installRabbitMq"; }
        }

        public override string Area
        {
            get { return "Installation"; }
        }

        public override string Description
        {
            get { return "Install RabbitMq"; }
        }

        public InstallRabbitMqCommand()
        {

            Action = parameters =>
            {
                string path;
                string password;
                if (!parameters.TryGet("path", out path)) return;
                if (!parameters.TryGet("pw", out password)) return;

                ConvertToPem(path, password);

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

            var pfxDirectory = file.Directory.FullName;

            var pemFileName = file.Name.Replace(file.Extension, ".pem");
            var keyFileName = file.Name.Replace(file.Extension, ".key");


            X509Certificate2 pfx;

            try
            {
                pfx = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
            catch (Exception ex)
            {
                throw new CertificateException("Could not open PFX. Perhaps the password is wrong", ex);
            }

            var rsa = (RSACryptoServiceProvider)pfx.PrivateKey;

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    _log.Info("Creating key file..");

                    var pemWriter = new PemWriter(streamWriter);
                    var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();

                    var path = Path.Combine(pfxDirectory, keyFileName);
                    File.WriteAllBytes(path, memoryStream.GetBuffer());

                    _log.Info(string.Format("Key file written to {0}", path));
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    _log.Info("Creating certificate file..");

                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(pfx));
                    streamWriter.Flush();

                    var path = Path.Combine(pfxDirectory, pemFileName);
                    File.WriteAllBytes(path, memoryStream.GetBuffer());

                    _log.Info(string.Format("PEM file written to {0}", path));
                }
            }

            _log.Info("Done");
        }

    }
}