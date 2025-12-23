using TicketService.Dtos.Events;
using TicketService.Entities.Implementations;

namespace TicketService.Events;

public class TicketEvents
{
    public static TicketCreatedEventDto TicketCreated(Ticket ticket) =>
        new(
            Guid.NewGuid(),
            DateTime.UtcNow,
            ticket.Id,
            ticket.Title,
            ticket.StatusId,
            ticket.PriorityId,
            ticket.ReporterId,
            ticket.AssigneeId
        );

    public static TicketAssignedEventDto TicketAssigned(Ticket ticket) =>
        new(
            Guid.NewGuid(),
            DateTime.UtcNow,
            ticket.Id,
            ticket.AssigneeId
        );

    public static TicketUpdatedEventDto TicketUpdated(Ticket ticket) =>
        new(
            Guid.NewGuid(),
            DateTime.UtcNow,
            ticket.Id,
            ticket.StatusId
        );
}