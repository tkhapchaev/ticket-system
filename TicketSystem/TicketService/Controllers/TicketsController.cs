using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketService.Dtos;
using TicketService.Entities;
using TicketService.Events;
using TicketService.Infrastructure;
using TicketService.Infrastructure.Interfaces;

namespace TicketService.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly TicketsDbContext _dbContext;
    private readonly IEventBus _eventBus;

    public TicketsController(TicketsDbContext dbContext, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _eventBus = eventBus;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
    {
        var ticket = new Ticket(dto.Title, dto.Description, (TicketPriority)dto.Priority, dto.ReporterEmail);
        _dbContext.Tickets.Add(ticket);
        
        await _dbContext.SaveChangesAsync();
        await _eventBus.PublishAsync("ticket.created", ticket.Id.ToString(), TicketEvents.TicketCreated(ticket));
        
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, TicketDtoMap.From(ticket));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ticket = await _dbContext.Tickets.FindAsync(id);
        return ticket is null ? NotFound() : Ok(TicketDtoMap.From(ticket));
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? status, [FromQuery] int? priority,
                                          [FromQuery] string? assignee, [FromQuery] int page = 1,
                                          [FromQuery] int pageSize = 20)
    {
        var queryable = _dbContext.Tickets.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            queryable = queryable.Where(x => x.Status == (TicketStatus)status.Value);
        }

        if (priority.HasValue)
        {
            queryable = queryable.Where(x => x.Priority == (TicketPriority)priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(assignee))
        {
            queryable = queryable.Where(x => x.AssigneeEmail == assignee);
        }

        var ticketDtos = await queryable.OrderByDescending(x => x.CreatedAt)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .Select(TicketDtoMap.FromExpression)
                           .ToListAsync();
        
        return Ok(ticketDtos);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto dto)
    {
        var ticket = await _dbContext.Tickets.FindAsync(id);
        
        if (ticket is null)
        {
            return NotFound();
        }
        
        ticket.UpdateCore(dto.Title, dto.Description, (TicketPriority)dto.Priority);
        await _dbContext.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpPost("{id:guid}/assign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Assign(Guid id, [FromBody] AssignDto dto)
    {
        var ticket = await _dbContext.Tickets.FindAsync(id);

        if (ticket is null)
        {
            return NotFound();
        }
        
        ticket.Assign(dto.AssigneeEmail);
        await _dbContext.SaveChangesAsync();
        await _eventBus.PublishAsync("ticket.assigned", ticket.Id.ToString(), TicketEvents.TicketAssigned(ticket));
        
        return NoContent();
    }

    [HttpPost("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] StatusDto dto)
    {
        var ticket = await _dbContext.Tickets.FindAsync(id);
        
        if (ticket is null)
        {
            return NotFound();
        }
        
        ticket.UpdateStatus((TicketStatus)dto.Status);
        await _dbContext.SaveChangesAsync();
        await _eventBus.PublishAsync("ticket.status.changed", ticket.Id.ToString(), TicketEvents.TicketStatusChanged(ticket));
        
        return NoContent();
    }
}