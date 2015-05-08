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
            SBUtils.Unit.SetLicenseKey("24F7A444489258824F89382597241909234F50C87BF401BF4ACA70B208911852E088414C4C58FD725251AFDA54A70B1CF8C90D11D62EE2944ADCE661BD7309D064ADA3A061919643DC21796F56A145EF5514B8DE55D3BFA5A590C891CC8E579CB49B60519C5940DF6F13274DC366DBFD2061454602437F252A6E0BC5902D7B07FC4427B36A5E9769A9216E10403AB49621DC8A774FED2E4032516A7F0425B791B6104CFFD529CCD0481F13A887A62405092CEC6D0C8465244964A72FBE565C705B7F0544C1EC5DDDD38F719C407777448844CA32EF8F29D0F8887C4938F51303CFD5E16509159522B7B8BC525C29E0751A84A86E87C251BAE45E96318B068D98");
        }
    }
}
