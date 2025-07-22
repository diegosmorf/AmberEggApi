using Api.Common.Contracts.Entities;
using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Cqrs.Core.Commands;
using Autofac;
using System.Threading.Tasks;

namespace AmberEggApi.Infrastructure.Bus;

public class InMemoryCommandConsumer(IComponentContext container) : ICommandConsumer
{
    private readonly IComponentContext container = container;

    public async Task<TEntity> Receive<TCommand, TEntity>(TCommand command)
        where TCommand : ICommand where TEntity : IDomainEntity
    {
        var handler = container.Resolve<ICommandHandler<TCommand, TEntity>>();
        var instance = await handler.Handle(command);

        return instance;
    }
}