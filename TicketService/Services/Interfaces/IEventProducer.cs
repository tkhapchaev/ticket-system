namespace TicketService.Services.Interfaces;

public interface IEventProducer
{
    Task PublishAsync(string topic, string key, object payload, CancellationToken cancellationToken);
}