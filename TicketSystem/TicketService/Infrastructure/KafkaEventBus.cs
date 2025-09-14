using System.Text.Json;
using Confluent.Kafka;
using TicketService.Infrastructure.Interfaces;

namespace TicketService.Infrastructure;

public class KafkaEventBus : IEventBus
{
    private readonly IProducer<string, string> _producer;
    
    public KafkaEventBus(IProducer<string, string> producer) => _producer = producer;

    public async Task PublishAsync(string topic, string key, object payload, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(payload);
        
        await _producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = key,
            Value = json
        }, cancellationToken);
    }
}