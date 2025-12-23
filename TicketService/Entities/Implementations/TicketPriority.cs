using TicketService.Entities.Interfaces;

namespace TicketService.Entities.Implementations;

public class TicketPriority : ITicketPriority
{
    private TicketPriority() { }

    public TicketPriority(int id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, "Ticket priority name cannot be null or white space");

        Id = id;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
}