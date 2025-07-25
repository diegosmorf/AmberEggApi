using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Entities;
using AmberEggApi.Contracts.Validations;

using System.Threading.Tasks;

namespace AmberEggApi.Infrastructure.Bus;

public class InMemoryCommandProducer(ICommandConsumer consumer) : ICommandProducer
{
    private readonly ICommandConsumer consumer = consumer;

    public async Task<TEntity> Send<TCommand, TEntity>(TCommand command)
        where TCommand : ICommand where TEntity : IDomainEntity
    {
        //Validating Command Model Attributes
        command.RaiseExceptionIfModelIsNotValid();
        //Calling Consumer InMemory
        return await consumer.Receive<TCommand, TEntity>(command);
    }
}