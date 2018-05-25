using System;
using System.Collections.Generic;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;

namespace Thycotic.RabbitMq.Helper.PSCommands.General
{
    /// <summary>
    ///     Selects the exception as well its inner exceptions. Optionally selects the stack trace
    /// </summary>
    /// <para type="synopsis"> Selects the exception as well its inner exceptions. Optionally selects the stack trace</para>
    /// <para type="description">The Select-Exception cmdlet enumerates the specified exception is generates a list of key-value pairs which could be selected further.</para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>try { Assert-RabbitMqConnectivity -UserName test15 -Password test15 -Hostname localhost } catch { Select-Exception $_.Exception}</code>
    /// </example>
    [Cmdlet(VerbsCommon.Select, "Exception")]
    public class SelectExceptionCommand : Cmdlet
    {

        /// <summary>
        ///     Gets or sets the exception.
        /// </summary>
        /// <value>
        ///     The Exception
        /// </value>
        /// <para type="description">Gets or sets the exception.</para>
        [Parameter(

            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("PfxPw")]
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the include stack trace.
        /// </summary>
        /// <value>
        /// The PFX path.
        /// </value>
        /// <para type="description">Gets or sets the include stack trace.</para>
        [Parameter(
             Mandatory = false,
             ValueFromPipeline = true,
             ValueFromPipelineByPropertyName = true)]
        public SwitchParameter IncludeStackTrace { get; set; }



        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var dump = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CombinedMessage", Exception.GetCombinedMessage())
            };

            var ex2 = Exception;
            var c = 0;
            while (ex2 != null)
            {
                dump.Add(new KeyValuePair<string, string>($"Message{c}", ex2.Message));

                if (IncludeStackTrace)
                {
                    dump.Add(new KeyValuePair<string, string>($"StackTrace{c}", ex2.StackTrace));
                }

                ex2 = ex2.InnerException;
                c++;
            }
            
            WriteObject(dump);
        }

    }
}