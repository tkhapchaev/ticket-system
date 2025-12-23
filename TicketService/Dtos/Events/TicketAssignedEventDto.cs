namespace TicketService.Dtos.Events;

public record TicketAssignedEventDto(
    Guid EventId,
    DateTime OccurredAt,
    Guid TicketId,
    Guid? AssigneeId
);