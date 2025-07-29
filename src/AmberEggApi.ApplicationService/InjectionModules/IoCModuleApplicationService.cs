using AmberEggApi.ApplicationService.Mapping;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.ApplicationService.InjectionModules;

public class IoCModuleApplicationService : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleApplicationService));

        builder.RegisterType<ObjectMapper>().AsImplementedInterfaces();

        builder
            .RegisterAssemblyTypes(assemblyToScan)
            .Where(c => c.IsClass
                        && c.IsInNamespace("AmberEggApi.ApplicationService.CommandHandlers")).AsImplementedInterfaces();

        builder
            .RegisterAssemblyTypes(assemblyToScan)
            .Where(c => c.IsClass
                        && c.IsInNamespace("AmberEggApi.ApplicationService.QueryHandlers")).AsImplementedInterfaces();

    }
}