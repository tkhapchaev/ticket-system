using Microsoft.EntityFrameworkCore;
using TicketService.Dtos.Mappers;
using TicketService.Dtos.Response;
using TicketService.Entities.Implementations;
using TicketService.Repositories;
using TicketService.Services.Interfaces;

namespace TicketService.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationContext _applicationContext;

    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationContext applicationContext, ILogger<UserService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task<UserDto> CreateAsync(string login)
    {
        var user = new User(login);

        _applicationContext.Users.Add(user);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation($"User {user.Login} (id: {user.Id}) created");

        return UserMapper.MapToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
        => await _applicationContext.Users
            .AsNoTracking()
            .Select(user => UserMapper.MapToDto(user))
            .ToListAsync();

    public async Task<UserDto?> GetByIdAsync(Guid id)
        => await _applicationContext.Users
            .AsNoTracking()
            .Where(user => user.Id == id)
            .Select(user => UserMapper.MapToDto(user))
            .FirstOrDefaultAsync();

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _applicationContext.Users.FindAsync(id);

        if (user == null)
            return false;

        _applicationContext.Users.Remove(user);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation($"User {user.Login} (id: {user.Id}) deleted");

        return true;
    }
}