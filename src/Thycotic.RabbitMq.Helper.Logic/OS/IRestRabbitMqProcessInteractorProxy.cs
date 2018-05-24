namespace Thycotic.RabbitMq.Helper.Logic.OS
{
    /// <summary>
    /// Interface Rest-based RabbitMq process interactor proxy
    /// </summary>
    public interface IRestRabbitMqProcessInteractorProxy
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the admin user.
        /// </summary>
        /// <value>
        /// The name of the admin user.
        /// </value>
        string AdminUserName { get; set; }

        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>
        /// The admin password.
        /// </value>
        string AdminPassword { get; set; }
    }
}