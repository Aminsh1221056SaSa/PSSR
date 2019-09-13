using Autofac;

namespace PSSR.Logic.App_Start
{
    public class AminLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Autowire the classes with interfaces
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();
        }
    }
}
