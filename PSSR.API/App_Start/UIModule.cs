using Autofac;
using PSSR.DbAccess.App_Start;
using PSSR.Logic.App_Start;
using PSSR.ServiceLayer.App_Start;

namespace PSSR.API.App_Start
{
    public class UIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //-----------------------------
            //Now register the other layers
            builder.RegisterModule(new ServiceLayerModule());
            builder.RegisterModule(new AminDbAccessModule());
            builder.RegisterModule(new AminLogicModule());
        }
    }
}
