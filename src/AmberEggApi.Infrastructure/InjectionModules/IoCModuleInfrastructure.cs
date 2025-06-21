using AmberEggApi.Database.Repositories;
using AmberEggApi.Infrastructure.Bus;
using Api.Common.Repository.EFCore;
using Api.Common.Repository.Repositories;
using Autofac;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.Infrastructure.InjectionModules
{
    public class IoCModuleInfrastructure : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Service Bus
            builder
                .RegisterType<InMemoryCommandProducer>()
                .AsImplementedInterfaces();

            builder
                .RegisterType<InMemoryCommandConsumer>()
                .AsImplementedInterfaces();

            // Infra - DbContext
            builder.RegisterType<EfCoreDbContext>().As<DbContext>();

            // Infra - Unit Of Work            
            builder
                .RegisterType<EFCoreUnitOfWork>()
                .As<IUnitOfWork>();

            // Infra - Repository
            builder
                .RegisterGeneric(typeof(EfCoreRepository<>))
                .AsImplementedInterfaces();

            //Registering all Infra services
            var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleInfrastructure));
            builder
                .RegisterAssemblyTypes(assemblyToScan)
                .Where(c => c.IsClass
                            && c.IsInNamespace("AmberEggApi.Infrastructure.Services")).AsImplementedInterfaces();
        }
    }
}
