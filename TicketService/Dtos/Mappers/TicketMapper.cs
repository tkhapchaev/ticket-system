using TicketService.Dtos.Response;
using TicketService.Entities.Interfaces;

namespace TicketService.Dtos.Mappers;

public class TicketMapper
{
    public static TicketDto MapToDto(ITicket ticket)
    {
        return new TicketDto(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.ReporterId,
            ticket.AssigneeId,
            ticket.PriorityId,
            ticket.StatusId,
            ticket.ProjectId,
            ticket.CreatedAt,
            ticket.UpdatedAt
        );
    }
}