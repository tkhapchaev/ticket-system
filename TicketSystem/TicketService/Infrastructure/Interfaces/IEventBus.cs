namespace TicketService.Infrastructure.Interfaces;

public interface IEventBus
{
    Task PublishAsync(string topic, string key, object payload, CancellationToken cancellationToken = default);
}