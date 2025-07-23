using AmberEggApi.ApplicationService.Mappings;
using Autofac;
using AutoMapper;

namespace AmberEggApi.ApplicationService.InjectionModules;
public class IoCModuleAutoMapper : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(
                context => new MapperConfiguration(
                    cfg => cfg.AddProfile(new DomainMapping()
                    )))
            .AsSelf().SingleInstance();

        builder.Register(
                c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
            .As<IMapper>()
            .InstancePerLifetimeScope();
    }
}