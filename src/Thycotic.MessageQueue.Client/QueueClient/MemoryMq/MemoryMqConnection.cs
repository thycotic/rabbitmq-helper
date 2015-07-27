﻿using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Thycotic.Logging;
using Thycotic.MessageQueue.Client.QueueClient.MemoryMq.Wcf;
using Thycotic.Messages.Common;

namespace Thycotic.MessageQueue.Client.QueueClient.MemoryMq
{
    /// <summary>
    /// Memory Mq Connection
    /// </summary>
    public class MemoryMqConnection : ICommonConnection
    {
        private readonly MemoryMqWcfServiceConnectionFactory _connectionFactory;
        private Lazy<IMemoryMqWcfServiceConnection> _connection;
        private bool _terminated;
        private Lazy<Version> _serverVersion; 

        private readonly ILogWriter _log = Log.Get(typeof(MemoryMqConnection));

        /// <summary>
        /// Gets or sets the connection created.
        /// </summary>
        /// <value>
        /// The connection created.
        /// </value>
        public EventHandler ConnectionCreated { get; set; }

        /// <summary>
        /// Holds the Memory MQ version retrieved from the server.
        /// </summary>
        public Version ServerVersion { get { return _serverVersion.Value; }}

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryMqConnection" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="useSsl">if set to <c>true</c> [use SSL].</param>
        public MemoryMqConnection(string url, string userName, string password, bool useSsl)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(url));
            //TODO: remove after username and password are no longer required
            Contract.Requires<ArgumentException>(userName != null);
            Contract.Requires<ArgumentException>(password != null);
            _connectionFactory = new MemoryMqWcfServiceConnectionFactory { Uri = url, UseSsl = useSsl, Username = userName, Password = password, RequestedHeartbeat = 300 };
            ResetConnection();
        }

        /// <summary>
        /// Resets the connection.
        /// </summary>
        public void ResetConnection()
        {
            CloseCurrentConnection();

            _connection = new Lazy<IMemoryMqWcfServiceConnection>(() =>
            {
                _log.Debug("Opening connection...");
                try
                {
                    var cn = _connectionFactory.CreateConnection();

                    _log.Debug(string.Format("Creating connection to {0}", _connectionFactory.HostName));

                    //if the connection closes recover it

                    cn.ConnectionShutdown += RecoverConnection;

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

            _serverVersion = new Lazy<Version>(() =>
            {
                using (var model = _connection.Value.CreateModel())
                {
                    _log.Debug("Retrieving server version");
                    return ((MemoryMqModel)model).GetServerVersion();
                }
            });


            //if there are subscribers that care to know when a connection is created, notify them
            if (ConnectionCreated != null)
            {
                Task.Delay(TimeSpan.FromMilliseconds(500))
                    .ContinueWith(task => ConnectionCreated(this, new EventArgs()));
            }

        }
        private void RecoverConnection(object connection, EventArgs reason)
        {
            //if this was actually requested, don't recover the connection and let it die
            if (_terminated)
            {
                return;
            }

            _log.Warn(string.Format("Connection closed because {0}", reason));
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

            do
            {
                try
                {
                    return _connection.Value.CreateModel();

                }

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

            if (!_connection.IsValueCreated) return;

            if (_connection.Value.IsOpen)
            {
                _log.Debug("Closing connection...");
                _connection.Value.Close(2 * 1000);
                _log.Debug("Connection closed");
            }

            _connection.Value.Dispose();
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
