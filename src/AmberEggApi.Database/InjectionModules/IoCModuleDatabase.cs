using AmberEggApi.Database.Migrators;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace AmberEggApi.Database.InjectionModules
{
    public class IoCModuleDatabase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Infra - Repository
            builder
                .RegisterType<MongoDbMigrator>()
                .AsImplementedInterfaces();


            //Registering all migrations
            var assemblyToScan = Assembly.GetAssembly(typeof(IoCModuleDatabase));
            builder
                .RegisterAssemblyTypes(assemblyToScan)
                .Where(c => c.IsClass
                            && c.IsInNamespace("AmberEggApi.Database.Migrations")).AsImplementedInterfaces();
        }
    }
}