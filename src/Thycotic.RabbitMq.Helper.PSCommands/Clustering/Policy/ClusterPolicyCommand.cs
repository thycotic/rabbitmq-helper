using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.PSCommands.Clustering.Policy
{

    /// <summary>
    /// Base class for a cluster policy
    /// </summary>
    /// <seealso cref="Thycotic.RabbitMq.Helper.PSCommands.RestManagementConsoleCmdlet" />
    public abstract class ClusterPolicyCommand : RestManagementConsoleCmdlet
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        /// <para type="description">Gets or sets the name.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the pattern.
        /// </summary>
        /// <value>
        ///     The pattern.
        /// </value>
        /// <para type="description">Gets or sets the pattern.</para>
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Pattern { get; set; }

        /// <summary>
        /// Priority for the policy. In the event that more than one policy can match a given exchange or queue, the policy with the greatest priority applies.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        /// <para type="description">
        /// Priority for the policy.In the event that more than one policy can match a given exchange or queue, the policy with the greatest priority applies.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateRange(1, 100)]
        public int Priority { get; set; } = 10;

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqRestClient(BaseUrl, AdminCredential.UserName,
                AdminCredential.GetNetworkCredential().Password);



            client.CreatePolicy(string.Empty, Name, GetPolicy());

            WriteVerbose("Policy created/updated");
        }

        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <returns></returns>
        protected abstract Logic.ManagementClients.Rest.Models.Policy GetPolicy();
    }
}