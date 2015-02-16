using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Http;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.ihawu.Business.DoubleLock.Cryptography.KeyTypes;
using Thycotic.SecretServerEngine2.Web.Common.Request;
using Thycotic.SecretServerEngine2.Web.Common.Response;

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
            const int SALT_LENGTH = 8;

            var saltProvider = new ByteSaltProvider();

            SymmetricKey symmetricKey;
            InitializationVector initializationVector;
            CreateSymmetricKeyAndIv(out symmetricKey, out initializationVector);
            var asymmetricEncryptor = new AsymmetricEncryptor();
            var saltedSymmetricKey = saltProvider.Salt(symmetricKey.Value, SALT_LENGTH);
            var encryptedSymmetricKey = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedSymmetricKey);
            var saltedInitializationVector = saltProvider.Salt(initializationVector.Value, SALT_LENGTH);
            var encryptedInitializationVector = asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedInitializationVector);
            
            return new EngineAuthenticationResponse
            {
                SymmetricKey = encryptedSymmetricKey,
                InitializationVector = encryptedInitializationVector
            };
        }

        private static readonly ConcurrentDictionary<string, EngineAuthenticationResponse> ApprovedRequests = new ConcurrentDictionary<string, EngineAuthenticationResponse>();

        [HttpPost]
        [Route("Authenticate")]
        public Task<EngineAuthenticationResponse> Authenticate(EngineAuthenticationRequest request)
        {
            //TODO: Validate client - talk to Ben
            //TODO: Ask Kevin if public key for the engine is enough ot should we also have a friendly name?

            var result = request.Version < ReleaseInformationHelper.GetVersionAsDouble()
                ? new EngineAuthenticationResponse {UpgradeNeeded = true}
                : ApprovedRequests.GetOrAdd(request.ExchangeName, key => GetClientKey(request.PublicKey, request.Version));

            return Task.FromResult(result);
        }
    }
}
