using Thycotic.ActiveDirectory.Core;

namespace Thycotic.DistributedEngine.Logic.Areas.ActiveDirectory
{
    internal static class DomainInfoExtensions
    {
        // ReSharper disable once RedundantNameQualifier
        public static Thycotic.ActiveDirectory.Core.DomainInfo ToCoreDomainInfo(this Messages.DE.Areas.ActiveDirectory.DomainInfo src)
        {
            return new DomainInfo
            {
                DistinguishedName = src.DistinguishedName,
                DomainName = src.DomainName,
                Password = src.Password,
                ProtocolVersion = src.ProtocolVersion,
                UserName = src.UserName,
                Port = src.Port,
                LdapTimeoutInSeconds = src.LdapTimeoutInSeconds,
                UseSecureLdap = src.UseSecureLdap
            };
        }
    }
}