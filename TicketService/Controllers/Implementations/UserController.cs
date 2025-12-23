using Microsoft.AspNetCore.Mvc;
using TicketService.Controllers.Abstractions;
using TicketService.Dtos.Request;
using TicketService.Services.Interfaces;

namespace TicketService.Controllers.Implementations;

public class UserController : ApiV1Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
        => Ok(await _userService.CreateAsync(createUserDto.Login));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _userService.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => await _userService.GetByIdAsync(id) is { } user ? Ok(user) : NotFound();

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => await _userService.DeleteAsync(id) ? NoContent() : NotFound();
}