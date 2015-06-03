using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Thycotic.DistributedEngine.InteractiveRunner.ConsoleCommands;
using Thycotic.Logging;

namespace Thycotic.DistributedEngine.CertificateHelper
{
    public class ConvertPfxToPemCommand : ConsoleCommandBase, IImmediateConsoleCommand
    {
        private readonly ILogWriter _log = Log.Get(typeof(ConvertPfxToPemCommand));

        public override string Name
        {
            get { return "convertPftToPem"; }
        }

        public override string Area {
            get { return "Conversion"; }
        }

        public override string Description
        {
            get { return "Converts a PFX cert to a pem/key combination. The files will be in the same folder as the PFX"; }
        }

        public ConvertPfxToPemCommand()
        {

            Action = parameters =>
            {
                _log.Info("Posting message to exchange");

                string path;
                string password;
                if (!parameters.TryGet("path", out path)) return;
                if (!parameters.TryGet("pw", out password)) return;
                
                ConvertToPem(path, password);

                _log.Debug("Conversion completed");

            };
        }

        private static void ConvertToPem(string pfxPath, string password)
        {

            var file = new FileInfo(pfxPath);

            if (file.Directory == null || !file.Directory.Exists || !file.Exists)
            {
                throw new ApplicationException("File does not exist");
            }

            var pfxDirectory = file.Directory.FullName;

            var fileName = file.Name;
            var pemFileName = file.Name.Replace(file.Extension, ".pem");
            var keyFileName = file.Name.Replace(file.Extension, ".key");

            var pfx = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            var rsa = (RSACryptoServiceProvider)pfx.PrivateKey;

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var pemWriter = new PemWriter(streamWriter);
                    var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();
                    File.WriteAllBytes(Path.Combine(pfxDirectory, keyFileName), memoryStream.GetBuffer());
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(pfx));
                    streamWriter.Flush();
                    File.WriteAllBytes(Path.Combine(pfxDirectory, pemFileName), memoryStream.GetBuffer());
                }
            }
        }

        
    }
}