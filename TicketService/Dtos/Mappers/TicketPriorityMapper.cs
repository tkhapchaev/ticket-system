using TicketService.Dtos.Response;
using TicketService.Entities.Interfaces;

namespace TicketService.Dtos.Mappers;

public class TicketPriorityMapper
{
    public static TicketPriorityDto MapToDto(ITicketPriority ticketPriority)
    {
        return new TicketPriorityDto(ticketPriority.Id, ticketPriority.Name);
    }
}