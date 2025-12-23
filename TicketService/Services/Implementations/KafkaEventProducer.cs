using System.Text.Json;
using Confluent.Kafka;
using TicketService.Services.Interfaces;

namespace TicketService.Services.Implementations;

public class KafkaEventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;

    private readonly JsonSerializerOptions _jsonOptions;

    public KafkaEventProducer(IProducer<string, string> producer)
    {
        _producer = producer;

        _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task PublishAsync(string topic, string key, object payload, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topic,"Topic cannot be null or white space");
        ArgumentException.ThrowIfNullOrWhiteSpace(key, "Key cannot be null or white space");

        ArgumentNullException.ThrowIfNull(payload, "Payload cannot be null");

        var json = JsonSerializer.Serialize(payload, _jsonOptions);

        await _producer.ProduceAsync(
            topic,
            new Message<string, string>
            {
                Key = key,
                Value = json
            },
            cancellationToken
        );
    }
}