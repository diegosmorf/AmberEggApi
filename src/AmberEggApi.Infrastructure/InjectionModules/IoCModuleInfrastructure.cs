using System.Reflection;
using Api.Common.Cqrs.Core.Messages;
using Api.Common.Repository.MongoDb;
using Api.Common.Repository.Repositories;
using Autofac;
using AmberEggApi.Infrastructure.Bus;
using Microsoft.AspNetCore.Http;
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
            builder
                .RegisterType<MongoDbContext>()
                .As<IMongoDbContext>()
                .InstancePerLifetimeScope();

            // Infra - Unit Of Work            
            builder
                .RegisterType<MongoDbUnitOfWork>()
                .As<IUnitOfWork>();

            // Infra - Repository
            builder
                .RegisterGeneric(typeof(MongoDbRepository<>))
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
