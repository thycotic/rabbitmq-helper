using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Certificate
{
    /// <summary>
    ///     Converts a Certificate Authority cert to a pem. The pem file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Converts a Certificate Authority cert to a pem.</para>
    /// <para type="description">The Convert-CaCertToPem cmdlet converts a Certificate Authority cert to a pem.</para>
    /// <para type="description">The pem file will be located in the Thycotic RabbitMq Site Connector folder.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Convert-PfxToPem</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <para>Convert-CaCertToPem -CaCertPath "$PSScriptRoot\..\Examples\sc.cer" -Verbose</para>
    /// </example>
    [Cmdlet(VerbsData.Convert, "CaCertToPem")]
    public class ConvertCaCerToPemCommand : Cmdlet
    {
        /// <summary>
        ///     The certificate path
        /// </summary>
        public static readonly string CertificatePath = Path.Combine(InstallationConstants.RabbitMq.ConfigurationPath,
            "ca.pem");

        /// <summary>
        ///     Gets or sets the ca cert path.
        /// </summary>
        /// <value>
        ///     The ca cert path.
        /// </value>
        /// <para type="description">Gets or sets the ca cert path.</para>
        [Parameter(
             Position = 0,
             Mandatory = true,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public string CaCertPath { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            ConvertToPem(CaCertPath);
        }

        [SuppressMessage("Microsoft.Contracts", "TestAlwaysEvaluatingToAConstant",
             Justification = "File info bogus warning")]
        private void ConvertToPem(string cacertpath)
        {
            WriteVerbose(string.Format("Attempting to convert {0} to .pem file...", cacertpath));

            var file = new FileInfo(cacertpath);

            if (file.Extension.ToLower() != ".cer")
                throw new ApplicationException("File is not .CER");


            if ((file.Directory == null) || !file.Directory.Exists || !file.Exists)
                throw new ApplicationException("File does not exist");

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
                    WriteVerbose("Creating certificate file..");

                    var pemWriter = new PemWriter(streamWriter);
                    pemWriter.WriteObject(DotNetUtilities.FromX509Certificate(cert));
                    streamWriter.Flush();

                    File.WriteAllBytes(CertificatePath, memoryStream.GetBuffer());

                    WriteVerbose(string.Format("Certificate file written to {0}", CertificatePath));
                }
            }
        }
    }
}