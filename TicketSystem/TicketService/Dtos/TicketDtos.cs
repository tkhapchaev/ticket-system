using System.Linq.Expressions;
using TicketService.Entities;

namespace TicketService.Dtos;

public record CreateTicketDto(string Title, string Description, int Priority, string ReporterEmail);
public record UpdateTicketDto(string Title, string Description, int Priority);
public record AssignDto(string AssigneeEmail);
public record StatusDto(int Status);

public record TicketDto(
    Guid Id,
    string Title,
    string Description,
    int Priority,
    string PriorityName,
    int Status,
    string StatusName,
    string ReporterEmail,
    string? AssigneeEmail,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public static class TicketDtoMap
{
    public static TicketDto From(Ticket ticket) => 
        new(ticket.Id,
            ticket.Title,
            ticket.Description,
            (int)ticket.Priority,
            ticket.Priority.ToString(),
            (int)ticket.Status,
            ticket.Status.ToString(),
            ticket.ReporterEmail,
            ticket.AssigneeEmail,
            ticket.CreatedAt,
            ticket.UpdatedAt);

    public static Expression<Func<Ticket, TicketDto>> FromExpression => ticket => 
        new TicketDto(ticket.Id,
            ticket.Title,
            ticket.Description,
            (int)ticket.Priority,
            ticket.Priority.ToString(),
            (int)ticket.Status,
            ticket.Status.ToString(),
            ticket.ReporterEmail,
            ticket.AssigneeEmail,
            ticket.CreatedAt,
            ticket.UpdatedAt);
}