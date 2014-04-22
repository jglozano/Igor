namespace Igor.Startup
{
    using System.Collections.Generic;
    using Annotations;
    using Helpers;
    using Ninject.Modules;
    using Simple.Web.Authentication;
    using Simple.Web.Ninject;

    [UsedImplicitly]
    public class NinjectStartup: NinjectStartupBase
    {
        protected override IEnumerable<INinjectModule> CreateModules()
        {
            yield return new IgorModule();
        }

        private class IgorModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IClientHelper>().To<ClientHelper>();
                Bind<ISettingsHelper>().To<SettingsHelper>();
                Bind<IAuthenticationProvider>().To<AuthenticationProvider>();
            }
        }
    }
}