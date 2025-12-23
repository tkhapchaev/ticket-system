using TicketService.Dtos.Response;

namespace TicketService.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateAsync(string login);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}