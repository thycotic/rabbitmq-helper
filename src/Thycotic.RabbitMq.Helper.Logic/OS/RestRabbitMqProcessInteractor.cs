using System;

namespace Thycotic.RabbitMq.Helper.Logic.OS
{
    /// <summary>
    /// Rest-based RabbitMq process interactor
    /// </summary>
    /// <seealso cref="IProcessInteractor" />
    public class RestRabbitMqProcessInteractor : IProcessInteractor, IRestRabbitMqProcessInteractorProxy
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the admin user.
        /// </summary>
        /// <value>
        /// The name of the admin user.
        /// </value>
        public string AdminUserName { get; set; }

        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>
        /// The admin password.
        /// </value>
        public string AdminPassword { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestRabbitMqProcessInteractor" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="adminUserName">Name of the admin user.</param>
        /// <param name="adminPassword">The admin password.</param>
        protected RestRabbitMqProcessInteractor(string baseUrl, string adminUserName, string adminPassword)
        {
            BaseUrl = baseUrl;
            AdminUserName = adminUserName;
            AdminPassword = adminPassword;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IProcessInteractor" /> is exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="estimatedProcessDuration">Duration of the estimated process.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string Invoke(string parameters, TimeSpan estimatedProcessDuration)
        {
            throw new NotImplementedException();
        }
    }
}
