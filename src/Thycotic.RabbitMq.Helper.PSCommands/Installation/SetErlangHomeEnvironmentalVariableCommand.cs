using System;
using System.Linq;
using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Installation
{
    /// <summary>
    ///     Sets the ERLANG_HOME environmental variable
    /// </summary>
    /// <para type="synopsis">Sets the ERLANG_HOME environmental variable</para>
    /// <para type="description">TODO: This is part of the longer cmdlet description.</para>
    /// <para type="description">TODO: Also part of the longer cmdlet description.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <para type="link">TODO: Get-Help</para>
    /// <example>
    ///     <para>TODO: This is part of the first example's introduction.</para>
    ///     <para>TODO: This is also part of the first example's introduction.</para>
    ///     <code>TODO: New-Thingy | Write-Host</code>
    ///     <para>TODO: This is part of the first example's remarks.</para>
    ///     <para>TODO: This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "ErlangHomeEnvironmentalVariable")]
    [OutputType(typeof(string))]
    public class SetErlangHomeEnvironmentalVariableCommand : Cmdlet
    {
        /// <summary>
        ///     The erlang home environmental variable name
        /// </summary>
        public const string ErlangHomeEnvironmentalVariableName = "ERLANG_HOME";

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteVerbose("Setting Erlang environmental variables");

            var targets = new[]
            {
                EnvironmentVariableTarget.Machine,
                EnvironmentVariableTarget.Process
            };

            targets.ToList().ForEach(t =>
                Environment.SetEnvironmentVariable(ErlangHomeEnvironmentalVariableName,
                    InstallationConstants.Erlang.InstallPath, t));

            //WriteObject(InstallationConstants.Erlang.InstallPath);
        }
    }
}