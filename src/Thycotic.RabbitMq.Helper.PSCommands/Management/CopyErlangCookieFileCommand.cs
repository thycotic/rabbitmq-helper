using System;
using System.IO;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Copies system cookie to user profile.
    /// </summary>
    /// <para type="synopsis">Copies system cookie to user profile.</para>
    /// <para type="description">The Copy-ErlangCookieFile cmdlet copies Erlang cookie.</para>
    /// <para type="description">Cookie is placed in the current user's profile.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Copy-ErlangCookieFile</code>
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
                
                if (File.Exists(InstallationConstants.Erlang.CookieSystemPath))
                {
                    if (File.Exists(InstallationConstants.Erlang.CookieUserProfilePath))
                    {
                        //remove readonly
                        File.SetAttributes(InstallationConstants.Erlang.CookieUserProfilePath, File.GetAttributes(InstallationConstants.Erlang.CookieUserProfilePath) & ~FileAttributes.ReadOnly);
                    }

                    File.Copy(InstallationConstants.Erlang.CookieSystemPath, InstallationConstants.Erlang.CookieUserProfilePath, true);

                    WriteVerbose($"System cookie copied to {InstallationConstants.Erlang.CookieUserProfilePath}");
                }
                else
                {
                    throw new ApplicationException($"System cookie path not found in {InstallationConstants.Erlang.CookieSystemPath}. Installation may fail.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to copy system cookie: {ex.Message}. Manual deletion might be necessary", ex);
            }
        }
    }
}