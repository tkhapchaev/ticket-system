using TicketService.Dtos.Response;
using TicketService.Entities.Interfaces;

namespace TicketService.Dtos.Mappers;

public class TicketStatusMapper
{
    public static TicketStatusDto MapToDto(ITicketStatus ticketStatus)
    {
        return new TicketStatusDto(ticketStatus.Id, ticketStatus.Name);
    }
}