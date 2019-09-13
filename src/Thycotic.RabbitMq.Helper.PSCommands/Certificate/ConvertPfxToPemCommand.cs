using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.Security.Cryptography;

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
    ///     <para>PS C:\></para> 
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
             Position = 0,
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string PfxPath { get; set; }

        /// <summary>
        ///     Gets or sets the PFX password. Username part is ignored.
        /// </summary>
        /// <value>
        ///     The credential for the PFX.
        /// </value>
        /// <para type="description">Gets or set the credential for the PFX. Username part is ignored.</para>
        [Parameter(
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public PSCredential PfxCredential { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            ConvertToPem(PfxPath, PfxCredential.GetNetworkCredential().Password);
        }

        [SuppressMessage("Microsoft.Contracts", "TestAlwaysEvaluatingToAConstant",
             Justification = "File info bogus warning")]
        private void ConvertToPem(string pfxPath, string password)
        {
            WriteVerbose(string.Format("Attempting to convert {0} to .pem file...", pfxPath));

            var file = new FileInfo(pfxPath);

            if (file.Extension.ToLower() != ".pfx")
                throw new Exception("File is not .PFX");


            if ((file.Directory == null) || !file.Directory.Exists || !file.Exists)
                throw new Exception("File does not exist");

            X509Certificate2 cert;

            try
            {
                cert = new X509Certificate2(pfxPath, password,
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not open PFX. Perhaps the password is wrong", ex);
            }

            var converter = CertificateConverterFactory.GetConverter(cert);

            WriteVerbose("Creating key file..");
            converter.SavePrivateKeyToPemKey(cert, KeyPath);
            WriteVerbose($"Key file written to {KeyPath}");

            WriteVerbose("Creating certificate file..");
            converter.SaveCertificateToPem(cert, CertificatePath);
            WriteVerbose($"Certificate file written to {CertificatePath}");
        }
    }
}