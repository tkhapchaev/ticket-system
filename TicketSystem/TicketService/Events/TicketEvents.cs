using TicketService.Entities;

namespace TicketService.Events;

public static class TicketEvents
{
    public static object TicketCreated(Ticket ticket) => new
    {
        eventId = Guid.NewGuid(),
        occurredAt = DateTime.UtcNow,
        ticket = new
        {
            id = ticket.Id,
            title = ticket.Title,
            status = (int)ticket.Status,
            statusName = ticket.Status.ToString(),
            priority = (int)ticket.Priority,
            priorityName = ticket.Priority.ToString(),
            reporterEmail = ticket.ReporterEmail,
            assigneeEmail = ticket.AssigneeEmail
        }
    };

    public static object TicketAssigned(Ticket ticket) => new
    {
        eventId = Guid.NewGuid(),
        occurredAt = DateTime.UtcNow,
        ticket = new
        {
            id = ticket.Id,
            title = ticket.Title,
            status = (int)ticket.Status,
            statusName = ticket.Status.ToString(),
            priority = (int)ticket.Priority,
            priorityName = ticket.Priority.ToString(),
            reporterEmail = ticket.ReporterEmail,
            assigneeEmail = ticket.AssigneeEmail
        }
    };

    public static object TicketStatusChanged(Ticket ticket) => new
    {
        eventId = Guid.NewGuid(),
        occurredAt = DateTime.UtcNow,
        ticket = new
        {
            id = ticket.Id,
            title = ticket.Title,
            status = (int)ticket.Status,
            statusName = ticket.Status.ToString(),
            priority = (int)ticket.Priority,
            priorityName = ticket.Priority.ToString(),
            reporterEmail = ticket.ReporterEmail,
            assigneeEmail = ticket.AssigneeEmail
        }
    };
}