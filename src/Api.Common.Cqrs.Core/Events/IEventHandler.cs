using System.Threading.Tasks;

namespace Api.Common.Cqrs.Core.Events
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}