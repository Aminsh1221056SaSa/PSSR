
using Autofac;

namespace PSSR.ServiceLayer.App_Start
{
    public class ServiceLayerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Autowire the classes with interfaces
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();

        }
    }
}
