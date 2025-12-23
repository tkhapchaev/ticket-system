namespace NotificationService.Dtos.Events;

public record TicketUpdatedEventDto(
    Guid EventId,
    DateTime OccurredAt,
    Guid TicketId,
    int StatusId
);