using System.Management.Automation;

namespace Thycotic.RabbitMq.Helper.PSCommands.Management
{
    /// <summary>
    ///     Base class for a management console command
    /// </summary>
    public abstract class RestManagementConsoleCmdlet : Cmdlet
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the admin user.
        /// </summary>
        /// <value>
        /// The name of the admin user.
        /// </value>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string AdminUserName { get; set; }

        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>
        /// The admin password.
        /// </value>
        [Parameter(
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string AdminPassword { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RestManagementConsoleCmdlet" /> class.
        /// </summary>
        protected RestManagementConsoleCmdlet()
        {
            BaseUrl = "http://localhost:15672";
            AdminUserName = AdminPassword = "guest";
        }

    }
}