using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Thycotic.Logging;

namespace Thycotic.MessageQueueClient.RabbitMq
{
    public class RabbitMqConnection : IDisposable, IRabbitMqConnection
    {
        private readonly ConnectionFactory _connectionFactory;
        private Lazy<IConnection> _connection;
        private bool _terminated;

        private readonly ILogWriter _log = Log.Get(typeof(RabbitMqConnection));

        public RabbitMqConnection(string url)
        {
            _connectionFactory = new ConnectionFactory { Uri = url, RequestedHeartbeat = 300 };
            ResetConnection();
        }

        private void ResetConnection()
        {
            CloseCurrentConnection();

            _connection = new Lazy<IConnection>(() =>
            {
                _log.Debug("Opening connection...");
                try
                {
                    var cn = _connectionFactory.CreateConnection();

                    _log.Info("Connection opened");

                    //if the connection closes recover it
                    cn.ConnectionShutdown += RecoverConnection;

                    return cn;
                }
                catch (Exception)
                {
                    //if there is an issue opening the channel, clean up and rethrow
                    CloseCurrentConnection();
                    throw;
                }
            });
        }

        private void RecoverConnection(IConnection connection, ShutdownEventArgs reason)
        {
            //if this was actually requested, don't recover the connection and let it die
            if (_terminated) return;

            _log.Warn(string.Format("Connection closed because {0}", reason));
            ResetConnection();

        }

        public IModel OpenChannel(int retryAttempts, int retryDelayMs, float retryDelayGrowthFactor)
        {
            var remainingRetryAttempts = retryAttempts;
            float retryDelay = retryDelayMs;

            do
            {
                try
                {
                    return _connection.Value.CreateModel();
                }
                catch (OperationInterruptedException ex)
                {
                    if (ex.ShutdownReason != null) _log.Debug(ex.ShutdownReason.ReplyText);

                    throw;
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

            if (!_connection.IsValueCreated || !_connection.Value.IsOpen) return;

            _log.Debug("Closing connection...");
            _connection.Value.Close(2 * 1000);
            _log.Debug("Connection closed");
        }

        public void Dispose()
        {
            _terminated = true;

            CloseCurrentConnection();
        }
    }
}
