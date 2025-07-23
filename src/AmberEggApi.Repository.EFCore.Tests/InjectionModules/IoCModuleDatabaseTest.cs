using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.Repository.EFCoreTests.InjectionModules;

public class IoCModuleDatabaseTest : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleDatabaseTest));

        builder
            .RegisterAssemblyTypes(assemblyToScan)
            .Where(c => c.IsClass
                        && c.IsInNamespace("AmberEggApi.Repository.EFCoreTests.Factories")).AsSelf();

    }
}