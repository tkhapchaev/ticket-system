using Microsoft.AspNetCore.Mvc;

namespace TicketService.Controllers.Abstractions;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class ApiV1Controller : ControllerBase { }