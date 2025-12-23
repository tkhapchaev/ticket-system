using TicketService.Dtos.Response;

namespace TicketService.Services.Interfaces;

public interface ITicketPriorityService
{
    Task<IEnumerable<TicketPriorityDto>> GetAllAsync();
}