using TicketService.Dtos.Request;
using TicketService.Dtos.Response;

namespace TicketService.Services.Interfaces;

public interface ITicketService
{
    Task<TicketDto> CreateAsync(CreateTicketDto dto, CancellationToken cancellationToken);
    Task<IEnumerable<TicketDto>> GetAllAsync();
    Task<TicketDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateTicketDto dto, CancellationToken cancellationToken);
    Task<bool> AssignAsync(Guid id, Guid? assigneeId, CancellationToken cancellationToken);
}