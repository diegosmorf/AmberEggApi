using Api.Common.Cqrs.Core.CommandHandlers;
using Api.Common.Cqrs.Core.Commands;
using Api.Common.Cqrs.Core.Entities;
using Api.Common.Cqrs.Core.Events;
using Autofac;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmberEggApi.Infrastructure.Bus
{
    public class InMemoryCommandConsumer(IComponentContext container) : ICommandConsumer
    {
        private readonly IComponentContext container = container;

        public async Task<TEntity> Receive<TCommand, TEntity>(TCommand command)
            where TCommand : ICommand where TEntity : IAggregateRoot
        {
            var handler = container.Resolve<ICommandHandler<TCommand, TEntity>>();
            var instance = await handler.Handle(command);

            PublishEvents(instance.AppliedEvents);
            return instance;
        }

        private void PublishEvents(IEnumerable<IEvent> appliedEvents)
        {
            foreach (var @event in appliedEvents)
            {
                var genericType = typeof(IEventHandler<>);
                var type = genericType.MakeGenericType(@event.GetType());
                var methodName = "Handle";
                var methodInfo = type.GetMethod(methodName);

                var handler = container.Resolve(type);
                methodInfo?.Invoke(handler, [@event]);
            }
        }
    }
}