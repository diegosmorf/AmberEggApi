namespace Api.Common.Cqrs.Core.Events
{
    public interface IEventConsumer
    {
        Task Receive<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}