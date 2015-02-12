using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Thycotic.SecretServerEngine2.Web.Models;

namespace Thycotic.SecretServerEngine2.Web.Controllers
{
    [RoutePrefix("api/EngineAuthentication")]
    public class EngineAuthenticationController : ApiController
    {
        [HttpPost]
        [Route("GetNewKey")]
        public Task<EngineAuthenticationRequest> GetNewKey()
        {
            KeyExchange.PrivateKey privateKey;
            KeyExchange.PublicKey publicKey;
            KeyExchange.CreatePublicAndPrivateKeys(out publicKey, out privateKey);

            return Task.FromResult(new EngineAuthenticationRequest{ PublicKey = Convert.ToBase64String(publicKey.Value) });
        }

        [HttpPost]
        [Route("Authenticate")]
        public Task<EngineAuthenticationResult> Authenticate(EngineAuthenticationRequest request)
        {
            return Task.FromResult(KeyExchange.GetClientKey(request.PublicKey, request.Version));
        }

        //// GET: api/EngineAuthentication
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/EngineAuthentication/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/EngineAuthentication
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/EngineAuthentication/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/EngineAuthentication/5
        //public void Delete(int id)
        //{
        //}
    }
}
