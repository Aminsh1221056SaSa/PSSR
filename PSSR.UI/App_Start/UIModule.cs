using Autofac;
using PSSR.DbAccess.App_Start;
using PSSR.Logic.App_Start;
using PSSR.ServiceLayer.App_Start;
using PSSR.UI.Helpers;
using PSSR.UI.Helpers.CashHelper;
using PSSR.UI.Helpers.ExcelHelper;

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

            builder.RegisterType<ExcelReportHelper>().As<IExcelReportHelper>()
            .InstancePerLifetimeScope();
        }
    }
}
