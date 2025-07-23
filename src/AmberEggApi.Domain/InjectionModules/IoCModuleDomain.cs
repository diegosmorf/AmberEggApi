using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.Domain.InjectionModules;
public class IoCModuleDomain : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleDomain));

        builder
            .RegisterAssemblyTypes(assemblyToScan)
            .Where(c => c.IsClass
                        && c.IsInNamespace("AmberEggApi.Domain.CommandHandlers")).AsImplementedInterfaces();        
    }
}