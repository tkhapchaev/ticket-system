using NotificationService.Services;

namespace NotificationService.Interfaces;

public interface INotificationSender
{
    Task SendAsync(TicketEvent ticketEvent, CancellationToken cancellationToken);
}