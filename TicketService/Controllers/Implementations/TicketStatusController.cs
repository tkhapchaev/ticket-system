using Microsoft.AspNetCore.Mvc;
using TicketService.Controllers.Abstractions;
using TicketService.Services.Interfaces;

namespace TicketService.Controllers.Implementations;

public class TicketStatusController : ApiV1Controller
{
    private readonly ITicketStatusService _ticketStatusService;

    public TicketStatusController(ITicketStatusService ticketStatusService)
    {
        _ticketStatusService = ticketStatusService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _ticketStatusService.GetAllAsync());
}