namespace TicketService.Dtos.Events;

public record TicketUpdatedEventDto(
    Guid EventId,
    DateTime OccurredAt,
    Guid TicketId,
    int StatusId
);