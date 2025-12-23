using Confluent.Kafka;
using Microsoft.Extensions.Options;
using NotificationService.Configurations;
using NotificationService.Dtos.Events;
using NotificationService.Services.Interfaces;
using System.Text.Json;

namespace NotificationService.Services.Implementations;

public class KafkaEventConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;

    private readonly INotificationSender _notificationSender;

    private readonly ILogger<KafkaEventConsumer> _logger;

    private readonly JsonSerializerOptions _jsonOptions;

    private readonly KafkaTopicOptions _kafkaTopicOptions;

    public KafkaEventConsumer(IConsumer<string, string> consumer, INotificationSender notificationSender, IOptions<KafkaTopicOptions> kafkaTopicOptions, ILogger<KafkaEventConsumer> logger)
    {
        _consumer = consumer;
        _notificationSender = notificationSender;
        _kafkaTopicOptions = kafkaTopicOptions.Value;
        _logger = logger;

        _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(
        [
            _kafkaTopicOptions.TicketCreated,
            _kafkaTopicOptions.TicketUpdated,
            _kafkaTopicOptions.TicketAssigned
        ]);

        _logger.LogInformation("Kafka consumer started");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);

                if (result == null)
                    continue;

                await HandleMessageAsync(result.Topic, result.Message.Value, stoppingToken);
                _consumer.Commit(result);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer stopping");
        }
        finally
        {
            _consumer.Close();
        }
    }

    private async Task HandleMessageAsync(string topic, string payload, CancellationToken cancellationToken)
    {
        if (topic == _kafkaTopicOptions.TicketCreated)
        {
            var ticketCreatedEventDto = JsonSerializer.Deserialize<TicketCreatedEventDto>(payload, _jsonOptions);
            await _notificationSender.SendAsync($"Ticket created: {ticketCreatedEventDto.TicketId}", cancellationToken);
        }
        else if (topic == _kafkaTopicOptions.TicketAssigned)
        {
            var ticketAssignedEventDto = JsonSerializer.Deserialize<TicketAssignedEventDto>(payload, _jsonOptions);
            await _notificationSender.SendAsync($"Ticket {ticketAssignedEventDto.TicketId} assigned to user {ticketAssignedEventDto.AssigneeId}", cancellationToken);
        }
        else if (topic == _kafkaTopicOptions.TicketUpdated)
        {
            var ticketUpdatedEventDto = JsonSerializer.Deserialize<TicketUpdatedEventDto>(payload, _jsonOptions);
            await _notificationSender.SendAsync($"Ticket updated: {ticketUpdatedEventDto.TicketId}", cancellationToken);
        }
    }
}