using Api.Common.Cqrs.Core.Commands;
using Api.Common.Cqrs.Core.Entities;
using Api.Common.Repository.Validations;
using System.Threading.Tasks;

namespace AmberEggApi.Infrastructure.Bus
{
    public class InMemoryCommandProducer(ICommandConsumer consumer) : ICommandProducer
    {
        private readonly ICommandConsumer consumer = consumer;

        public async Task<TEntity> Send<TCommand, TEntity>(TCommand command)
            where TCommand : ICommand where TEntity : IAggregateRoot
        {
            //Validating Command Model Attributes
            command.RaiseExceptionIfModelIsNotValid();

            //Here you can save into any queue or message broker of your preference

            //Calling Consumer InMemory
            return await consumer.Receive<TCommand, TEntity>(command);
        }
    }
}