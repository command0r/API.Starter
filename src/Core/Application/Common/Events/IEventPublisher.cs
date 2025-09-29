using API.Starter.Shared.Events;

namespace API.Starter.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}