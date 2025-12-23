using Microsoft.EntityFrameworkCore;
using TicketService.Dtos.Mappers;
using TicketService.Dtos.Response;
using TicketService.Repositories;
using TicketService.Services.Interfaces;

namespace TicketService.Services.Implementations;

public class TicketPriorityService : ITicketPriorityService
{
    private readonly ApplicationContext _applicationContext;

    public TicketPriorityService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IEnumerable<TicketPriorityDto>> GetAllAsync()
        => await _applicationContext.TicketPriorities
            .AsNoTracking()
            .Select(ticketPriority => TicketPriorityMapper.MapToDto(ticketPriority))
            .ToListAsync();
}