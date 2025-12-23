using TicketService.Dtos.Response;
using TicketService.Entities.Interfaces;

namespace TicketService.Dtos.Mappers;

public class ProjectMapper
{
    public static ProjectDto MapToDto(IProject project)
    {
        return new ProjectDto(project.Id, project.Name);
    }
}