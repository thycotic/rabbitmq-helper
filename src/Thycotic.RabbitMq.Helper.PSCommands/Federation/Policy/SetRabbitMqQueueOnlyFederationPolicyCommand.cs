using System.Management.Automation;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.PSCommands.Federation.Policy
{
    /// <summary>
    ///     Creates a federation policy on the RabbitMq node
    /// </summary>
    /// <para type="synopsis">Creates a federation policy on the RabbitMq node</para>
    /// <para type="description"></para>
    /// <para type="link" uri="http://www.thycotic.com">Thycotic Software Ltd</para>
    /// <example>
    ///     <para>PS C:\></para> 
    ///     <code>Set-RabbitMqFederationPolicyCommand</code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "RabbitMqQueueOnlyFederationPolicy")]
    public class SetRabbitMqQueueOnlyFederationPolicyCommand : PolicyCommand
    {
        /// <inheritdoc />
        protected override Logic.ManagementClients.Rest.Models.Policy GetPolicy()
        {
            return new Logic.ManagementClients.Rest.Models.Policy
            {
                pattern = Pattern,
                applyTo = PolicyOptions.PolicyApplications.Queues,
                //definition = new FederationPolicyDefinition
                //{
                //    federation_upstream_set = PolicyOptions.FederationUpstreamSets.All
                //},
                priority = Priority
                
            };
        }
    }
}