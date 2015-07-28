using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Thycotic.Logging;

namespace Thycotic.MessageQueue.Client.QueueClient.RabbitMq
{
    /// <summary>
    /// Rabbit Mq Connection
    /// </summary>
    public class RabbitMqConnection : ICommonConnection
    {
        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        public EventHandler ConnectionCreated { get; set; }

        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private Version _serverVersion;
        private bool _terminated;

        private readonly ILogWriter _log = Log.Get(typeof(RabbitMqConnection));

        /// <summary>
        /// Holds the Rabbit MQ version retrieved from the server.
        /// </summary>
        public string ServerVersion
        {
            get { return _serverVersion.ToString(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConnection" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public RabbitMqConnection(string url, string userName, string password, bool useSsl)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(url));
            Contract.Requires<ArgumentException>(userName != null);
            Contract.Requires<ArgumentException>(password != null);

            _connectionFactory = new ConnectionFactory
            {
                Uri = url,
                RequestedHeartbeat = 300,
                UserName = userName,
                Password = password
            };

            if (useSsl)
            {
                var uri = new Uri(url);

                _connectionFactory.Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = uri.Host,
                    //AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors,
                };
            }

            ResetConnection();

        }


        #region Mapping
        private static ICommonModel Map(IModel createModel)
        {
            return new RabbitMqModel(createModel);
        }
        #endregion

        private void ResetConnection()
        {
            CloseCurrentConnection();


            _log.Debug("Opening connection...");
            try
            {
                var cn = _connectionFactory.CreateConnection();

                GetServerVersion(cn);

                _log.Info(string.Format("Connection opened to {0}", _connectionFactory.HostName));

                //if the connection closes recover it
                cn.ConnectionShutdown += RecoverConnection;

                _connection = cn;
            }
            catch (Exception ex)
            {
                _log.Warn(string.Format("Encountered issue connecting to {0}. Will reconnect in {1}ms. {2} Use DEBUG logging for more details.", _connectionFactory.HostName, DefaultConfigValues.ReOpenDelay, ex.Message));
                _log.Debug(string.Format("Encountered issue connecting to {0}. Will reconnect in {1}ms", _connectionFactory.HostName, DefaultConfigValues.ReOpenDelay), ex);

                Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(task => ResetConnection());

                throw;
            }
        }

        private void GetServerVersion(IConnection cn)
        {
            object version;
            cn.ServerProperties.TryGetValue("version", out version);

            if (version == null)
            {
                throw new ApplicationException("Version could not be determined");
            }

            _serverVersion = Version.Parse(System.Text.Encoding.UTF8.GetString((byte[])version));
        }

        private void RecoverConnection(IConnection connection, ShutdownEventArgs reason)
        {
            //if this was actually requested, don't recover the connection and let it die
            if (_terminated)
            {
                return;
            }

            _log.Warn(string.Format("Recovering connection because {0}", reason != null ? reason.ToString() : "Reason unknown"));
            ResetConnection();

        }

        /// <summary>
        /// Opens the channel with the specified retry attempts, retry delay and retry delay growth factor
        /// </summary>
        /// <param name="retryAttempts">The retry attempts.</param>
        /// <param name="retryDelayMs">The retry delay ms.</param>
        /// <param name="retryDelayGrowthFactor">The retry delay growth factor.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Channel should have opened</exception>
        public ICommonModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor)
        {
            var remainingRetryAttempts = retryAttempts;
            float retryDelay = retryDelayMs;

            if (_connection == null || !_connection.IsOpen)
            {
                throw new ApplicationException("No open connection available");
            }

            do
            {
                try
                {

                    return Map(_connection.CreateModel());
                }
                catch (OperationInterruptedException ex)
                {
                    if (ex.ShutdownReason != null) _log.Debug(ex.ShutdownReason.ReplyText);

                    throw;
                }
                catch (Exception ex)
                {
                    _log.Error(
                        string.Format("Failed to open a channel, {0} retry attempts remaining",
                            remainingRetryAttempts), ex);

                    //too many retries, just stop
                    if (remainingRetryAttempts == 0) throw;

                    //modeled after Binary Exponential back off. The more initialization fails, the longer we wait to retry or we ultimately fail
                    Thread.Sleep((Convert.ToInt32(retryDelay)));
                    retryDelay *= retryDelayGrowthFactor;
                    if (remainingRetryAttempts != -1)
                    {
                        remainingRetryAttempts--;
                    }
                }
            } while (remainingRetryAttempts > 0 || remainingRetryAttempts == -1);


            throw new ApplicationException("Channel should have opened");
        }

        private void CloseCurrentConnection()
        {
            if (_connection == null)
            {
                return;
            }

            if (_connection.IsOpen)
            {
                _log.Debug("Closing connection...");
                _connection.Close(2 * 1000);
                _log.Debug("Connection closed");
            }

            //HACK: Current version of Rabbit API seems to hang dispose when the connection was closed by server
            if (_connection.CloseReason.ReplyCode != 320)
            {
                _connection.Dispose();
            }
            _connection = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _terminated = true;

            CloseCurrentConnection();
        }
    }
}
