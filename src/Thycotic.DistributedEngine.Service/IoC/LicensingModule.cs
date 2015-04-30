using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Thycotic.DistributedEngine.Logic.Areas.POC;
using Thycotic.DistributedEngine.Logic.Licensing;
using Thycotic.DistributedEngine.Logic.Licensing.Providers;
using Thycotic.Logging;
using Module = Autofac.Module;

namespace Thycotic.DistributedEngine.Service.IoC
{
    class LicensingModule : Module
    {
        private readonly Dictionary<string, string> _thycoticKeys;
        private readonly Dictionary<string, string> _thirdPartyKeys;

        private readonly ILogWriter _log = Log.Get(typeof(LicensingModule));
        
        public LicensingModule(Dictionary<string, string> thycoticKeys, Dictionary<string, string> thirdPartyKeys)
        {
            _thycoticKeys = thycoticKeys;
            _thirdPartyKeys = thirdPartyKeys;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            using (LogContext.Create("Licensing keys"))
            {
                builder.Register(content => new ThycoticLicenseKeyProvider(_thycoticKeys)).As<IThycoticLicenseKeyProvider>().SingleInstance();
                builder.Register(content => new ThirdPartyLicenseKeyProvider(_thycoticKeys)).As<IThirdPartyLicenseKeyProvider>().SingleInstance();
            }

            using (LogContext.Create("Library licensing"))
            {
                var logicAssembly = Assembly.GetAssembly(typeof(HelloWorldConsumer));
                
                builder.RegisterAssemblyTypes(logicAssembly)
                    .Where(t => t.IsAssignableTo<ILibraryLicenser>())
                    .As<IStartable>();
            }
        }
    }
}
