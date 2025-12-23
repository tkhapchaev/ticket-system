using TicketService.Dtos.Response;

namespace TicketService.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateAsync(string name);
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(Guid id);
}