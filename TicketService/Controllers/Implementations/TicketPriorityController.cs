using Microsoft.AspNetCore.Mvc;
using TicketService.Controllers.Abstractions;
using TicketService.Services.Interfaces;

namespace TicketService.Controllers.Implementations;

public class TicketPriorityController : ApiV1Controller
{
    private readonly ITicketPriorityService _ticketPriorityService;

    public TicketPriorityController(ITicketPriorityService ticketPriorityService)
    {
        _ticketPriorityService = ticketPriorityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _ticketPriorityService.GetAllAsync());
}