namespace TicketService.Entities.Interfaces;

public interface IUser
{
    Guid Id { get; }
    string Login { get; }
}