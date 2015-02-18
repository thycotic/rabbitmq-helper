using System;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    /// <summary>
    /// Rabbit Mq Connection
    /// </summary>
    public class MemoryMqConnection : ICommonConnection
    {
        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        public EventHandler ConnectionCreated { get; set; }

        private readonly MemoryMqServiceConnectionFactory _connectionFactory;
        private Lazy<IMemoryMqServiceConnection> _connection;
        private bool _terminated;

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqConnection));

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqConnection"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="useSsl"></param>
        public MemoryMqConnection(string url, bool useSsl)
        {
            _connectionFactory = new MemoryMqServiceConnectionFactory { Uri = url, UseSsl = useSsl, RequestedHeartbeat = 300 };
            ResetConnection();
        }

        private void ResetConnection()
        {
            CloseCurrentConnection();

            _connection = new Lazy<IMemoryMqServiceConnection>(() =>
            {
                _log.Debug("Opening connection...");
                try
                {
                    var cn = _connectionFactory.CreateConnection();

                    _log.Debug(string.Format("Connection opened to {0}", _connectionFactory.HostName));

                    //if the connection closes recover it
                    cn.ConnectionShutdown += RecoverConnection;

                    //if there are subscribers that care to know when a connection is created, notify them
                    if (ConnectionCreated != null)
                    {
                        Task.Delay(TimeSpan.FromMilliseconds(500))
                            .ContinueWith(task => ConnectionCreated(this, new EventArgs()));
                    }

                    return cn;
                }
                catch (Exception ex)
                {
                    //if there is an issue opening the channel, clean up and rethrow
                    _log.Error(string.Format("Failed to connect because {0}", ex.Message));

                    _log.Info("Sleeping before reconnecting");

                    Task.Delay(DefaultConfigValues.ReOpenDelay).ContinueWith(task => ResetConnection());

                    throw;
                }
            });
        }

        private void RecoverConnection(object connection, EventArgs reason)
        {
            //if this was actually requested, don't recover the connection and let it die
            if (_terminated) return;

            _log.Warn(string.Format("Connection closed because {0}", reason));
            ResetConnection();

        }

        /// <summary>
        /// Forces the initialization.
        /// </summary>
        /// <returns></returns>
        public bool ForceInitialize()
        {
            return _connection.Value != null;
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

            do
            {
                try
                {
                    return _connection.Value.CreateModel();
                }
                //catch (OperationInterruptedException ex)
                //{
                //    if (ex.ShutdownReason != null) _log.Debug(ex.ShutdownReason.ReplyText);

                //    throw;
                //}
                catch (Exception ex)
                {
                    _log.Error(string.Format("Failed to open a channel, {0} retry attempts remaining", remainingRetryAttempts), ex);

                    //too many retries, just stop
                    if (remainingRetryAttempts == 0) throw;

                    //modeled after Binary Exponential back off. The more initialization fails, the longer we wait to retry or we ultimately fail
                    Thread.Sleep((Convert.ToInt32(retryDelay)));
                    retryDelay *= retryDelayGrowthFactor;
                    remainingRetryAttempts--;
                }
            } while (remainingRetryAttempts > 0);

            throw new ApplicationException("Channel should have opened");
        }

        private void CloseCurrentConnection()
        {
            if (_connection == null)
            {
                return;
            }

            if (!_connection.IsValueCreated || !_connection.Value.IsOpen) return;

            _log.Debug("Closing connection...");
            _connection.Value.Close(2 * 1000);
            _log.Debug("Connection closed");
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
