using System;
using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Cli;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering
{
    /// <summary>
    ///     Sets the Erlang cookie file contents
    /// </summary>
    /// <para type="synopsis">  Sets the Erlang cookie file contents</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Set-ErlangCookieFileCommand -CookieContent "monster"</code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "ErlangCookieFileCommand")]
    public class SetErlangCookieFileCommand : Cmdlet
    {
        /// <summary>
        /// Gets or sets the content of the cookie.
        /// </summary>
        /// <value>
        /// The content of the cookie.
        /// </value>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string CookieContent { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var cookieContent = CookieContent.Trim();

            if (string.IsNullOrWhiteSpace(cookieContent))
            {
                throw new Exception("No valid cookie content specified");
            }

            var client = new RabbitMqBatCtlClient();

            WriteVerbose("Stopping RabbitMq node");
            client.HardStop();

            try
            {
                try
                {

                    WriteVerbose($"Setting system cookie contents {InstallationConstants.Erlang.CookieSystemPath}");

                    //remove readonly
                    File.SetAttributes(InstallationConstants.Erlang.CookieSystemPath,
                        File.GetAttributes(InstallationConstants.Erlang.CookieSystemPath) & ~FileAttributes.ReadOnly);

                    File.WriteAllText(InstallationConstants.Erlang.CookieSystemPath, cookieContent);

                    //add readonly
                    File.SetAttributes(InstallationConstants.Erlang.CookieSystemPath,
                        File.GetAttributes(InstallationConstants.Erlang.CookieSystemPath) & FileAttributes.ReadOnly);
                }
                catch (Exception ex2)
                {
                    throw new Exception("Failed to set system cookie content.", ex2);
                }

                try
                {
                    WriteVerbose(
                        $"Setting user profile cookie contents {InstallationConstants.Erlang.CookieUserProfilePath}");

                    //remove readonly
                    File.SetAttributes(InstallationConstants.Erlang.CookieUserProfilePath,
                        File.GetAttributes(InstallationConstants.Erlang.CookieUserProfilePath) &
                        ~FileAttributes.ReadOnly);

                    File.WriteAllText(InstallationConstants.Erlang.CookieUserProfilePath, cookieContent);

                    //add readonly
                    File.SetAttributes(InstallationConstants.Erlang.CookieUserProfilePath,
                        File.GetAttributes(InstallationConstants.Erlang.CookieUserProfilePath) &
                        FileAttributes.ReadOnly);
                }
                catch (Exception ex2)
                {
                    throw new Exception("Failed to set user profile cookie content.", ex2);
                }

                WriteVerbose("Cookie set");

                WriteVerbose("Starting RabbitMq");
                client.SoftStart();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to set cookie content for clustering. Manual copy and paste is most likely required", ex);
            }
        }
    }
}