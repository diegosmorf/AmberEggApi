//using AmberEggApi.ApplicationService.InjectionModules;
//using AmberEggApi.Database.InjectionModules;
//using AmberEggApi.Infrastructure.InjectionModules;
//using Autofac;
//using NUnit.Framework;
//using System.Threading.Tasks;

//namespace Api.Common.WebServer.Tests
//{
//    [SetUpFixture]
//    public class SetupTest
//    {
//        public static IContainer Container { get; protected set; }

//        [OneTimeSetUp]
//        public async Task RunBeforeAllTests()
//        {
//            // Setup IoC Container
//            var builder = new ContainerBuilder();
//            builder.RegisterModule(new IoCModuleApplicationService());
//            builder.RegisterModule(new IoCModuleInfrastructure());
//            builder.RegisterModule(new IoCModuleAutoMapper());
//            builder.RegisterModule(new IoCModuleDatabase());

//            Container = builder.Build();
//        }

//        [OneTimeTearDown]
//        public void RunAfterAllTests()
//        {
//            Container.Dispose();
//        }
//    }
//}