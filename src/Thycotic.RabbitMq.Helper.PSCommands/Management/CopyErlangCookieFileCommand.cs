using System;
using System.IO;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Copies RabbitMq SSL example configuration file. The configuration file will be located in the Thycotic RabbitMq Site Connector folder.
    /// </summary>
    /// <para type="synopsis">Copies system cookie to user profile.</para>
    /// <para type="description">The Copy-ErlangCookieFile cmdlet copies Erlang cookie.</para>
    /// <para type="description">Cookie is places in the current user's profile.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">Copy-RabbitMqExampleNonSslConfigFile</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Copy-RabbitMqExampleSslConfigFile</code>
    /// </example>
    [Cmdlet(VerbsCommon.Copy, "ErlangCookieFile")]
    public class CopyErlangCookieFileCommand : Cmdlet
    {
        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                const string filename = ".erlang.cookie";
                var windowsCookiePath = Path.Combine(@"C:\Windows\System32\config\systemprofile\", filename);
                if (File.Exists(windowsCookiePath))
                {
                    var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    var userProfileCookiePath = Path.Combine(userProfile, filename);

                    if (File.Exists(userProfileCookiePath))
                    {
                        File.Delete(userProfileCookiePath);
                    }

                    File.Copy(windowsCookiePath, userProfileCookiePath);

                    WriteVerbose($"System cookie copied to {userProfileCookiePath}");
                }
                else
                {
                    throw new ApplicationException($"System cookie path not found in {windowsCookiePath}. Installation may fail.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to copy system cookie: {ex.Message}", ex);
            }
        }
    }
}