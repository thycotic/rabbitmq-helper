using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Certificate
{
    /// <summary>
    ///     Converts a PFX cert to a pem/key combination. The pem file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Converts a PFX cert to a pem/key combination.</para>
    /// <para type="description">The Convert-PfxToPem cmdlet converts a PFX cert to a pem/key combination.</para>
    /// <para type="description">The pem file will be located in the Thycotic RabbitMq Site Connector folder.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Convert-CaCerToPem</para>
    /// <example>
    ///     <code>Convert-PfxToPem -PfxPath "$PSScriptRoot\..\Examples\sc.pfx" -PfxPassword "password1" -Verbose</code>
    /// </example>
    [Cmdlet(VerbsData.Convert, "PfxToPem")]
    public class ConvertPfxToPemCommand : Cmdlet
    {
        /// <summary>
        ///     The certificate path
        /// </summary>
        public static readonly string CertificatePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
            "cert.pem");

        /// <summary>
        ///     The key path
        /// </summary>
        public static readonly string KeyPath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
            "cert.key");

        /// <summary>
        ///     Gets or sets the PFX path.
        /// </summary>
        /// <value>
        ///     The PFX path.
        /// </value>
        /// <para type="description">Gets or sets the PFX path.</para>
        [Parameter(
             Mandatory = true,
             Position = 0,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string PfxPath { get; set; }

        /// <summary>
        ///     Gets or sets the PFX password.
        /// </summary>
        /// <value>
        ///     The PFX password.
        /// </value>
        /// <para type="description">Gets or sets the PFX password.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        [Alias("PfxPw")]
        public string PfxPassword { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            ConvertToPem(PfxPath, PfxPassword);
        }

        [SuppressMessage("Microsoft.Contracts", "TestAlwaysEvaluatingToAConstant",
             Justification = "File info bogus warning")]
        private void ConvertToPem(string pfxPath, string password)
        {
            WriteVerbose(string.Format("Attempting to convert {0} to .pem file...", pfxPath));

            var file = new FileInfo(pfxPath);

            if (file.Extension.ToLower() != ".pfx")
                throw new ApplicationException("File is not .PFX");


            if ((file.Directory == null) || !file.Directory.Exists || !file.Exists)
                throw new ApplicationException("File does not exist");

            X509Certificate2 cert;

            try
            {
                cert = new X509Certificate2(pfxPath, password,
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
            catch (Exception ex)
            {
                throw new CertificateException("Could not open PFX. Perhaps the password is wrong", ex);
            }

            var rsa = (RSACryptoServiceProvider) cert.PrivateKey;

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    WriteVerbose("Creating key file..");

                    var pemWriter = new PemWriter(streamWriter);
                    var keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                    pemWriter.WriteObject(keyPair.Private);
                    streamWriter.Flush();


                    File.WriteAllBytes(KeyPath, memoryStream.GetBuffer());

                    WriteVerbose(string.Format("Key file written to {0}", KeyPath));
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    WriteVerbose("Creating certificate file..");

                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(cert));
                    streamWriter.Flush();

                    File.WriteAllBytes(CertificatePath, memoryStream.GetBuffer());

                    WriteVerbose(string.Format("Certificate file written to {0}", CertificatePath));
                }
            }

            WriteVerbose("Done");
        }
    }
}