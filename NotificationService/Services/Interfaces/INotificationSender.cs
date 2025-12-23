namespace NotificationService.Services.Interfaces;

public interface INotificationSender
{
    Task SendAsync(string message, CancellationToken cancellationToken);
}