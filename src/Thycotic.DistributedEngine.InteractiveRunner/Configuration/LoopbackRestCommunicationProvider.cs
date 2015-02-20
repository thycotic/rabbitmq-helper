using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Web.Common;
using Thycotic.DistributedEngine.Web.Common.Request;
using Thycotic.DistributedEngine.Web.Common.Response;
using Thycotic.ihawu.Business.Agents;

namespace Thycotic.DistributedEngine.InteractiveRunner.Configuration
{
    internal class LoopbackRestCommunicationProvider : IRestCommunicationProvider
    {
        private readonly ByteSaltProvider _saltProvider = new ByteSaltProvider();
        private readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        private readonly Dictionary<Uri, Func<object, dynamic>> _loopBacks = new Dictionary<Uri, Func<object, dynamic>>();

        public LoopbackRestCommunicationProvider()
        {
            const string prefix = EndPoints.EngineWebService.Prefix;

            _loopBacks.Add(this.GetEndpointUri(prefix, EndPoints.EngineWebService.Actions.GetConfiguration),
                LoopbackNotSupported);
            _loopBacks.Add(this.GetEndpointUri(prefix, EndPoints.EngineWebService.Actions.Authenticate),
                LoopbackAuthenticate);
        }

        public TResult Post<TResult>(Uri uri, object request)
        {
            if (_loopBacks.ContainsKey(uri))
            {
                return _loopBacks[uri].Invoke(request);
            }

            throw new NotSupportedException(string.Format("No loopback configured for {0}", uri));
        }

        private byte[] EncryptWithPublicKey(string publicKey, byte[] bytes)
        {
            var saltedBytes = _saltProvider.Salt(bytes, RpcAgentServer.SALT_LENGTH);
            return _asymmetricEncryptor.EncryptWithPublicKey(new PublicKey(Convert.FromBase64String(publicKey)), saltedBytes);
        }

        private static object LoopbackNotSupported(object request)
        {
            throw new NotSupportedException();
        }

        private EngineAuthenticationResponse LoopbackAuthenticate(object request)
        {
            var authRequest = (EngineAuthenticationRequest)request;

            const int aesKeySize = 256;
            const int ivSize = 128;

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = ivSize;
                aes.KeySize = aesKeySize;
                aes.GenerateIV();
                aes.GenerateKey();

                return new EngineAuthenticationResponse
                {
                    Success = true,
                    SymmetricKey = EncryptWithPublicKey(authRequest.PublicKey, aes.Key),
                    InitializationVector = EncryptWithPublicKey(authRequest.PublicKey, aes.IV)
                };
            }
        }
    }
}