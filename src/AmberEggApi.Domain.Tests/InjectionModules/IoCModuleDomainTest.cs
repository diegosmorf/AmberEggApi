using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.DomainTests.InjectionModules;

public class IoCModuleDomainTest : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleDomainTest));

        builder
            .RegisterAssemblyTypes(assemblyToScan)
            .Where(c => c.IsClass
                        && c.IsInNamespace("AmberEggApi.DomainTests.Factories")).AsSelf();

        builder
            .RegisterAssemblyTypes(Assembly.Load("AmberEggApi.WebApi"))
            .Where(c => c.IsClass
                        && c.IsInNamespace("AmberEggApi.WebApi.Controllers")).AsSelf();

    }
}