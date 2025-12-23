using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TicketService.Dtos.Mappers;
using TicketService.Dtos.Response;
using TicketService.Repositories;
using TicketService.Services.Interfaces;

namespace TicketService.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly ApplicationContext _applicationContext;

    private readonly ILogger<ProjectService> _logger;

    public ProjectService(ApplicationContext applicationContext, ILogger<ProjectService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task<ProjectDto> CreateAsync(string name)
    {
        var project = new Entities.Implementations.Project(name);

        _applicationContext.Projects.Add(project);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation($"Project {project.Name} (id: {project.Id}) created");

        return ProjectMapper.MapToDto(project);
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        => await _applicationContext.Projects
            .Include(project => project.Tickets)
            .AsNoTracking()
            .Select(project => ProjectMapper.MapToDto(project))
            .ToListAsync();

    public async Task<ProjectDto?> GetByIdAsync(Guid id)
        => await _applicationContext.Projects
            .AsNoTracking()
            .Where(project => project.Id == id)
            .Select(project => ProjectMapper.MapToDto(project))
            .FirstOrDefaultAsync();
}