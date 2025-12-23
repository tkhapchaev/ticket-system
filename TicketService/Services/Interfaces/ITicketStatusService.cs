using TicketService.Dtos.Response;

namespace TicketService.Services.Interfaces;

public interface ITicketStatusService
{
    Task<IEnumerable<TicketStatusDto>> GetAllAsync();
}