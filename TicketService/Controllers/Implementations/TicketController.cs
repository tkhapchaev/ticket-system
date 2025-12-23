using Microsoft.AspNetCore.Mvc;
using TicketService.Controllers.Abstractions;
using TicketService.Dtos.Request;
using TicketService.Services.Interfaces;

namespace TicketService.Controllers.Implementations;

public class TicketController : ApiV1Controller
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService service)
    {
        _ticketService = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketDto createTicketDto, CancellationToken cancellationToken)
        => Ok(await _ticketService.CreateAsync(createTicketDto, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _ticketService.GetAllAsync());

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTicketDto updateTicketDto, CancellationToken cancellationToken)
        => await _ticketService.UpdateAsync(id, updateTicketDto, cancellationToken) ? NoContent() : NotFound();

    [HttpPatch("{id:guid}/assign")]
    public async Task<IActionResult> Assign(Guid id, AssignTicketDto assignTicketDto, CancellationToken cancellationToken)
        => await _ticketService.AssignAsync(id, assignTicketDto.AssigneeId, cancellationToken) ? NoContent() : NotFound();
}