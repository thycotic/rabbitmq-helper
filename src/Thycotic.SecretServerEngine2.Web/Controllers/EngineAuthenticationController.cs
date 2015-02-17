using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;
using Thycotic.MessageQueueClient;
using Thycotic.SecretServerEngine2.Web.Common.Request;
using Thycotic.SecretServerEngine2.Web.Common.Response;
using Thycotic.Utility;
using Thycotic.Utility.Security;
using Thycotic.Utility.Serialization;

namespace Thycotic.SecretServerEngine2.Web.Controllers
{
    [RoutePrefix("api/EngineAuthentication")]
    public class EngineAuthenticationController : ApiController
    {
        public static void CreateSymmetricKeyAndIv(out SymmetricKey symmetricKey, out InitializationVector initializationVector)
        {
            const int aesKeySize = 256;
            const int ivSize = 128;

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = ivSize;
                aes.KeySize = aesKeySize;
                aes.GenerateIV();
                aes.GenerateKey();
                symmetricKey = new SymmetricKey(aes.Key);
                initializationVector = new InitializationVector(aes.IV);
            }
        }

        public static EngineAuthenticationResponse GetClientKey(string publicKey, double version)
        {
            var saltProvider = new ByteSaltProvider();

            SymmetricKey symmetricKey;
            InitializationVector initializationVector;
            CreateSymmetricKeyAndIv(out symmetricKey, out initializationVector);
            var asymmetricEncryptor = new AsymmetricEncryptor();
            var saltedSymmetricKey = saltProvider.Salt(symmetricKey.Value, MessageEncryption.SaltLength);
            var encryptedSymmetricKey = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedSymmetricKey);
            var saltedInitializationVector = saltProvider.Salt(initializationVector.Value, MessageEncryption.SaltLength);
            var encryptedInitializationVector = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedInitializationVector);
            
            return new EngineAuthenticationResponse
            {
                SymmetricKey = encryptedSymmetricKey,
                InitializationVector = encryptedInitializationVector
            };
        }


        public static EngineConfigurationResponse GetConfiguration(string publicKey, double version)
        {
            var saltProvider = new ByteSaltProvider();

            var serializer = new JsonObjectSerializer();

            var configuration = new Dictionary<string, string>
            {
                {ConfigurationKeys.QueueType, SupportedMessageQueues.MemoryMq},
                {ConfigurationKeys.QueueExchangeName, "thycotic"},
                //{ConfigurationKeys.RabbitMq.ConnectionString, "amqp://THYCOPAIR24.testparent.thycotic.com:5671"},
                //ConfigurationKeys.RabbitMq.ConnectionString, "amqp://localhost:5671"},
                //{ConfigurationKeys.RabbitMq.UserName, "test"},
                //{ConfigurationKeys.RabbitMq.Password, "test"},
                //{ConfigurationKeys.RabbitMq.UseSSL, "false"},

                //{ConfigurationKeys.MemoryMq.ConnectionString, "net.tcp://THYCOPAIR24.testparent.thycotic.com:8523"},
                {ConfigurationKeys.MemoryMq.ConnectionString, "net.tcp://AURORA:8523"},
                {ConfigurationKeys.MemoryMq.UseSSL, "true"},
                //{ConfigurationKeys.MemoryMq.Server.Thumbprint, "f1faa2aa00f1350edefd9490e3fc95017db3c897"},
                {ConfigurationKeys.MemoryMq.Server.Thumbprint, "1ec85a6084862addedb77c4a777c86747f488c90"},
                {ConfigurationKeys.MemoryMq.Server.Start, "true"}
            };

            var configurationBytes = serializer.ToBytes(configuration);

            var asymmetricEncryptor = new AsymmetricEncryptor();
            var saltedConfiguration = saltProvider.Salt(configurationBytes, MessageEncryption.SaltLength);
            var encryptedConfiguration = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedConfiguration);
            
            return new EngineConfigurationResponse
            {
                Configuration = encryptedConfiguration
            };
        }


        private static readonly ConcurrentDictionary<string, EngineAuthenticationResponse> ApprovedRequests = new ConcurrentDictionary<string, EngineAuthenticationResponse>();

        [HttpPost]
        [Route("GetConfiguration")]
        public Task<EngineConfigurationResponse> GetConfiguration(EngineConfigurationRequest request)
        {
            var result = request.Version < ReleaseInformationHelper.GetVersionAsDouble()
                ? new EngineConfigurationResponse {UpgradeNeeded = true}
                : GetConfiguration(request.PublicKey, request.Version);

            return Task.FromResult(result);
        }

        [HttpPost]
        [Route("Authenticate")]
        public Task<EngineAuthenticationResponse> Authenticate(EngineAuthenticationRequest request)
        {
            var result = request.Version < ReleaseInformationHelper.GetVersionAsDouble()
                ? new EngineAuthenticationResponse { UpgradeNeeded = true }
                : ApprovedRequests.GetOrAdd(request.ExchangeName, key => GetClientKey(request.PublicKey, request.Version));

            return Task.FromResult(result);
        }
    }
}
