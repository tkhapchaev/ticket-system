namespace NotificationService.Dtos.Events;

public record TicketCreatedEventDto(
    Guid EventId,
    DateTime OccurredAt,
    Guid TicketId,
    string Title,
    int StatusId,
    int PriorityId,
    Guid ReporterId,
    Guid? AssigneeId
);