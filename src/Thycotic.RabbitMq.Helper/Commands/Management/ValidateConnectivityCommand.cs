using System;
using RabbitMQ.Client;
using Thycotic.Logging;
using Thycotic.RabbitMq.Helper.Commands.Installation;

namespace Thycotic.RabbitMq.Helper.Commands.Management
{
    internal class ValidateConnectivityCommand : ManagementConsoleCommandBase
    {

        private readonly ILogWriter _log = Log.Get(typeof(InstallRabbitMqCommand));

        public override string Area
        {
            get { return CommandAreas.Management; }
        }

        public override string Description
        {
            get { return "Adds a basic user to RabbitMq"; }
        }

        public ValidateConnectivityCommand()
        {

            Action = parameters =>
            {
                bool skipUserCreate;
                if (parameters.TryGetBoolean("skipUserCreate", out skipUserCreate) && skipUserCreate)
                {
                    _log.Info("User creation was skipped. Will not validate connection");
                    return 0;
                }

                string username;
                string password;
                if (!parameters.TryGet("rabbitMqUsername", out username))
                {
                    _log.Error("RabbitMq username is required");
                    return 1;
                }
                if (!parameters.TryGet("rabbitMqPw", out password))
                {
                    _log.Error("RabbitMq password is required");
                    return 1;
                }

                bool useSsl;
                parameters.TryGetBoolean("useSsl", out useSsl);

                try
                {
                    using (var connection = GetConnection(username, password, useSsl))
                    {
                        using (var model = connection.CreateModel())
                        {
                            if (model.IsOpen)
                            {
                                _log.Info("Connection successful");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Connection failed", ex);
                    return 1;
                }

                return 0;

            };
        }

        private IConnection GetConnection(string userName, string password, bool useSsl)
        {
            const int nonSslPort = 5672;
            const int sslPost = 5671;
            var url = string.Format("amqp://localhost:{0}", useSsl ? sslPost : nonSslPort);

            _log.Info(string.Format("Getting connection for {0}", url));

            var connectionFactory = new ConnectionFactory
            {
                Uri = url,
                RequestedHeartbeat = 300,
                UserName = userName,
                Password = password
            };

            if (useSsl)
            {
                var uri = new Uri(url);

                connectionFactory.Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = uri.Host,
                    //AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors,
                };
            }

            return connectionFactory.CreateConnection();

        }
    }
}