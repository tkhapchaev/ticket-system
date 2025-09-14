using NotificationService.Interfaces;

namespace NotificationService.Services;

public enum TicketStatus
{
    Open = 0,
    InProgress = 1,
    Resolved = 2,
    Closed = 3
}

public class LogNotificationSender : INotificationSender
{
    private readonly ILogger<LogNotificationSender> _logger;
    
    public LogNotificationSender(ILogger<LogNotificationSender> logger) => _logger = logger;

    public Task SendAsync(TicketEvent ticketEvent, CancellationToken cancellationToken)
    {
        var tp = ticketEvent.Ticket!;
        var title = tp.Title ?? "(no title)";
        var to = tp.AssigneeEmail ?? tp.ReporterEmail ?? "(unknown)";

        var subject = tp.Status switch
        {
            (int)TicketStatus.Open       => $"Ticket created: {title}",
            (int)TicketStatus.InProgress => $"Ticket in progress: {title}",
            (int)TicketStatus.Resolved   => $"Ticket resolved: {title}",
            (int)TicketStatus.Closed     => $"Ticket closed: {title}",
            _                            => $"Ticket update: {title}"
        };

        _logger.LogInformation(
            "Notify '{To}': {Subject} (status={Code}/{Name}, priority={PCode}/{PName})",
            to, subject, tp.Status, tp.StatusName ?? "?", tp.Priority, tp.PriorityName ?? "?");

        return Task.CompletedTask;
    }
}