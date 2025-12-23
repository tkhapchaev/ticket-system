using Microsoft.AspNetCore.Mvc;
using TicketService.Controllers.Abstractions;
using TicketService.Dtos.Request;
using TicketService.Services.Interfaces;

namespace TicketService.Controllers.Implementations;

public class ProjectController : ApiV1Controller
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto createProjectDto)
        => Ok(await _projectService.CreateAsync(createProjectDto.Name));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _projectService.GetAllAsync());
}