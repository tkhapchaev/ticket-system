using TicketService.Dtos.Response;
using TicketService.Entities.Interfaces;

namespace TicketService.Dtos.Mappers;

public class UserMapper
{
    public static UserDto MapToDto(IUser user)
    {
        return new UserDto(user.Id, user.Login);
    }
}