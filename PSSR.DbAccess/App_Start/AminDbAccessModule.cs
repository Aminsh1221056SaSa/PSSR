using Autofac;

namespace PSSR.DbAccess.App_Start
{
    public class AminDbAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Autowire the classes with interfaces
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();
        }
    }
}
