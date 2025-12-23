using NotificationService.Services.Interfaces;

namespace NotificationService.Services.Implementations;

public class LogNotificationSender : INotificationSender
{
    private readonly ILogger<LogNotificationSender> _logger;

    public LogNotificationSender(ILogger<LogNotificationSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Notification sent: {Message}", message);

        return Task.CompletedTask;
    }
}