using System.Threading;
using NUnit.Framework;
using Thycotic.DistributedEngine.Logic.Areas.Authentication;
using Thycotic.Messages.DE.Areas.Authenticate.Request;

namespace Thycotic.DistributedEngine.Logic.Tests.Areas.Authentication
{
    [TestFixture]
    public class AuthenticateByAdConsumerTests
    {
        [Test]
        [TestCase("testparent.thycotic.com", false, "mrmittens", "Password1", 389, "testparent.thycotic.com", true)]
        [TestCase("testparent.thycotic.com", false, "resetme", "invalidpassword", 389, "testparent.thycotic.com", false)]
        [TestCase("testparent.thycotic.com", false, "CatLord", "password1", 389, "testchild.testparent.thycotic.com", true)]
        [TestCase("testparent.thycotic.com", false, "userForEngineTests", "invalidpassword", 389, "testchild.testparent.thycotic.com", false)]
        [TestCase("testparent.thycotic.com", true, "mrmittens", "Password1", 636, "testparent.thycotic.com", true)]
        [TestCase("testparent.thycotic.com", true, "resetme", "invalidpassword", 636, "testparent.thycotic.com", false)]
        [TestCase("testparent.thycotic.com", true, "CatLord", "password1", 389, "testchild.testparent.thycotic.com", true)]
        [TestCase("testparent.thycotic.com", true, "userForEngineTests", "invalidpassword", 389, "testchild.testparent.thycotic.com", false)]
        [TestCase("testparent.thycotic.com", false, "mrmittens", "Password1", 999, "testparent.thycotic.com", false)]
        public void ShouldAuthenticateWithAd(string domainToAuthenticateWith, bool ldaps, string username, string password, int port, string userDomain, bool result)
        {
            var message = new AuthenticateByAdMessage
            {
                DomainToAuthenticateTo = domainToAuthenticateWith,
                Ldaps = ldaps,
                Username = username,
                Password = password,
                Port = port,
                UserDomain = userDomain
            };

            var response = new AuthenticateByAdConsumer().Consume(CancellationToken.None, message);
            Assert.AreEqual(result, response.Success, response.ErrorMessage);
        }             
    }
}
