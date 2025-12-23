using TicketService.Entities.Interfaces;

namespace TicketService.Entities.Implementations;

public class User : IUser
{
    private User() { }

    public User(string login)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(login, "User login cannot be null or white space");

        Login = login;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Login { get; private set; }
}