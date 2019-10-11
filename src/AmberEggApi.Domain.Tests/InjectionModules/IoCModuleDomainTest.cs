using Autofac;
using AmberEggApi.Domain.Tests.UnitTests;
using System.Reflection;
using AmberEggApi.Domain.Tests.Factories;
using Module = Autofac.Module;

namespace AmberEggApi.Domain.Tests.InjectionModules
{
    public class IoCModuleDomainTest : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleDomainTest));

            builder
                .RegisterAssemblyTypes(assemblyToScan)
                .Where(c => c.IsClass
                            && c.IsInNamespace("AmberEggApi.Domain.Tests.Factories")).AsSelf();

        }
    }
}