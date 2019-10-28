using Autofac;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;

namespace PSSR.UI.App_Start
{
    public class UIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MasterDataCacheOperations>().As<IMasterDataCacheOperations>()
                .SingleInstance();

            builder.RegisterType<NavigationCacheOperations>().As<INavigationCacheOperations>()
               .SingleInstance();

            builder.RegisterType<AdminNavigationHelper>().As<IAdminNavigationHelper>()
               .SingleInstance();

            builder.RegisterType<DatabaseService>().As<IDatabaseService>()
               .InstancePerLifetimeScope();
        }
    }
}
