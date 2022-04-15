using System.Threading.Tasks;

namespace Api.Common.Cqrs.Core.Events
{
    public interface IEventProducer
    {
        Task Send<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}