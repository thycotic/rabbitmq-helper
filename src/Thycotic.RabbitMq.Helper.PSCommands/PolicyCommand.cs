using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.PSCommands
{

    /// <summary>
    /// Base class for a policy cmdlet
    /// </summary>
    /// <seealso cref="Thycotic.RabbitMq.Helper.PSCommands.RestManagementConsoleCmdlet" />
    public abstract class PolicyCommand : RestManagementConsoleCmdlet
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
        /// Policy will apply to queues.
        /// </summary>
        /// <value>
        /// The policy will apply to queues.
        /// </value>
        /// <para type="description">
        /// Policy will apply to queues.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public SwitchParameter ApplyToQueues { get; set; } = true;

        /// <summary>
        /// Policy will apply to exchanges.
        /// </summary>
        /// <value>
        /// The policy will apply to exchanges.
        /// </value>
        /// <para type="description">
        /// Policy will apply to exchanges.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public SwitchParameter ApplyToExchanges { get; set; }


        /// <summary>
        /// Policy will include the matching targets in federation.
        /// </summary>
        /// <value>
        /// The policy will include the matching targets in federation.
        /// </value>
        /// <para type="description">
        /// Policy will include the matching targets in federation.
        /// </para>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public SwitchParameter IncludeInFederation { get; set; }

        /// <summary>
        ///     Processes the record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var client = new RabbitMqRestClient(BaseUrl, AdminCredential.UserName,
                AdminCredential.GetNetworkCredential().Password);

            var policy = GetPolicy(GetPolicyDefinition());
            client.CreatePolicy(Constants.RabbitMq.DefaultVirtualHost, Name, policy);

            WriteVerbose("Policy settings:");
            policy.definition.OrderBy(pd => pd.Key).ToList().ForEach(dv => WriteVerbose($"{dv.Key} = {dv.Value}"));

            WriteVerbose("Policy created/updated");
            
        }

        /// <summary>
        /// Gets the policy definition.
        /// </summary>
        /// <returns></returns>
        protected abstract IDictionary<string, object> GetPolicyDefinition();

        private Policy GetPolicy(IDictionary<string, object> definition)
        {
            var policy = new Policy
            {
                pattern = Pattern,
                definition = definition,
                priority = Priority
            };

            SetApplyTo(policy);

            SetIncludeInFederation(policy);

            return policy;
        }

        private void SetApplyTo(Policy policy)
        {
            string applyTo;
            if (ApplyToExchanges && ApplyToQueues)
            {
                applyTo = PolicyOptions.PolicyApplications.All;
            }
            else if (ApplyToExchanges)
            {
                applyTo = PolicyOptions.PolicyApplications.Exchanges;
            }
            else if (ApplyToQueues)
            {
                applyTo = PolicyOptions.PolicyApplications.Queues;
            }
            else
            {
                throw new Exception("Policy has to apply to queues, exchanges or both");
            }

            policy.applyTo = applyTo;
        }


        private void SetIncludeInFederation(Policy policy)
        {
            if (IncludeInFederation)
            {
                policy.definition.Add(PolicyOptions.PolicyKeys.FederationUpstreamSet, PolicyOptions.FederationUpstreamSets.All);
            }
        }
    }
}