using System.Collections.Generic;
using Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest.Models;

namespace Thycotic.RabbitMq.Helper.Logic.ManagementClients.Rest
{
    /// <summary>
    /// Interface for a rest management client
    /// </summary>
    public interface IRabbitMqRestClient
    {
        /// <summary>
        /// Deletes the queue.
        /// </summary>
        /// <param name="vhost">The vhost.</param>
        /// <param name="name">The name.</param>
        void DeleteQueue(string vhost, string name);
        /// <summary>
        /// Gets all queues.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Queue> GetAllQueues();

        /// <summary>
        /// Gets the name of the cluster.
        /// </summary>
        /// <returns></returns>
        string GetClusterName();
        /// <summary>
        /// Gets the cluster nodes.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Node> GetClusterNodes();
        /// <summary>
        /// Gets the health check.
        /// </summary>
        /// <returns></returns>
        NodeHealthCheck GetHealthCheck();
    }
}