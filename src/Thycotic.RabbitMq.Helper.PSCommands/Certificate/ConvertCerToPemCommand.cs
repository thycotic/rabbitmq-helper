using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Thycotic.RabbitMq.Helper.PSCommands.Installation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Certificate
{
    /// <summary>
    ///     Converts a Certificate Authority cert to a pem.
    /// </summary>
    /// <para type="synopsis">TODO: This is the cmdlet synopsis.</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://tempuri.org">TODO: Thycotic</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
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
        /// <para type="description">TODO: Property description.</para>
        [Parameter(
             Mandatory = true,
             Position = 0,
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

            WriteVerbose("Done");
        }
    }
}