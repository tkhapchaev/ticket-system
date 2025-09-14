using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using NotificationService.Interfaces;

namespace NotificationService.Services;

public sealed class TicketEvent
{
    [JsonPropertyName("eventId")] public Guid EventId { get; set; }
    [JsonPropertyName("occurredAt")] public DateTime OccurredAt { get; set; }
    [JsonPropertyName("ticket")] public TicketPayload? Ticket { get; set; }
}

public sealed class TicketPayload
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("status")] public int Status { get; set; }
    [JsonPropertyName("statusName")] public string? StatusName { get; set; }
    [JsonPropertyName("priority")] public int Priority { get; set; }
    [JsonPropertyName("priorityName")] public string? PriorityName { get; set; }
    [JsonPropertyName("reporterEmail")] public string? ReporterEmail { get; set; }
    [JsonPropertyName("assigneeEmail")] public string? AssigneeEmail { get; set; }
}

public class NotificationWorker : BackgroundService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };
    
    private readonly ILogger<NotificationWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly INotificationSender _sender;
    private IConsumer<string, string>? _consumer;

    public NotificationWorker(ILogger<NotificationWorker> logger, IConfiguration configuration, INotificationSender sender)
    {
        _logger = logger;
        _configuration = configuration;
        _sender = sender;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var bootstrap = _configuration["Kafka:BootstrapServers"];
        var groupId = _configuration["Kafka:GroupId"] ?? "notification-service";
        var adminConfig = new AdminClientConfig { BootstrapServers = bootstrap };
        
        using (var admin = new AdminClientBuilder(adminConfig).Build())
        {
            var topics = new[]
            {
                new TopicSpecification { Name = "ticket.created", NumPartitions = 1, ReplicationFactor = 1 },
                new TopicSpecification { Name = "ticket.assigned", NumPartitions = 1, ReplicationFactor = 1 },
                new TopicSpecification { Name = "ticket.status.changed", NumPartitions = 1, ReplicationFactor = 1 }
            };

            try
            {
                await admin.CreateTopicsAsync(topics, new CreateTopicsOptions
                {
                    RequestTimeout = TimeSpan.FromSeconds(10),
                    OperationTimeout = TimeSpan.FromSeconds(10)
                });
            }
            catch (CreateTopicsException exception)
            {
                foreach (var r in exception.Results)
                {
                    if (r.Error.Code != ErrorCode.TopicAlreadyExists)
                    {
                        _logger.LogWarning("Create topic '{Topic}' failed: {Error}", r.Topic, r.Error.Reason);
                    }
                }
            }
        }
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = _configuration["Kafka:GroupId"] ?? "notification-service",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _consumer.Subscribe(["ticket.created", "ticket.assigned", "ticket.status.changed"]);
        
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_consumer is null)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var ticketEvent = JsonSerializer.Deserialize<TicketEvent>(consumeResult.Message.Value, JsonSerializerOptions);
                
                if (ticketEvent?.Ticket is null)
                {
                    _logger.LogWarning("Skip invalid message on {Topic}/{Partition}@{Offset}: payload={Payload}", consumeResult.Topic, consumeResult.Partition, consumeResult.Offset, consumeResult.Message.Value);
                    _consumer.Commit(consumeResult);
                    continue;
                }
                
                var t = ticketEvent.Ticket;
                if (string.IsNullOrWhiteSpace(t.Title) || t.StatusName is null || t.PriorityName is null)
                {
                    _logger.LogWarning("Skip message with missing fields: {Payload}", consumeResult.Message.Value);
                    _consumer.Commit(consumeResult);
                    continue;
                }

                await _sender.SendAsync(ticketEvent, stoppingToken);
                _consumer.Commit(consumeResult);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Cancellation requested");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }
        }
    }

    public override void Dispose()
    {
        _consumer?.Close();
        _consumer?.Dispose();
        base.Dispose();
    }
}