using System;
using System.Collections.Generic;
using Thycotic.AppCore;
using Thycotic.AppCore.Cryptography;
using Thycotic.DistributedEngine.Logic;
using Thycotic.DistributedEngine.Web.Common;
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

        
    }
}