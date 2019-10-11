using AmberEggApi.Domain.InjectionModules;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.ApplicationService.InjectionModules
{
    public class IoCModuleApplicationService : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Domain Modules: Command and CommandHandlers
            builder.RegisterModule<IoCModuleDomain>();

            var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleApplicationService));

            builder
                .RegisterAssemblyTypes(assemblyToScan)
                .Where(c => c.IsClass
                            && c.IsInNamespace("AmberEggApi.ApplicationService.Services")).AsImplementedInterfaces();
            
        }
    }
}