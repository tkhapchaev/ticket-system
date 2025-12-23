using Microsoft.EntityFrameworkCore;
using TicketService.Dtos.Mappers;
using TicketService.Dtos.Response;
using TicketService.Repositories;
using TicketService.Services.Interfaces;

namespace TicketService.Services.Implementations;

public class TicketStatusService : ITicketStatusService
{
    private readonly ApplicationContext _applicationContext;

    public TicketStatusService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<IEnumerable<TicketStatusDto>> GetAllAsync()
        => await _applicationContext.TicketStatuses
            .AsNoTracking()
            .Select(ticketStatus => TicketStatusMapper.MapToDto(ticketStatus))
            .ToListAsync();
}