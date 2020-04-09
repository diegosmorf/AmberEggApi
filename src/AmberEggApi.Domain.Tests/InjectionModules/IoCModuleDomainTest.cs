using Autofac;
using System.Reflection;
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