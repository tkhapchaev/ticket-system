using TicketService.Entities.Interfaces;

namespace TicketService.Entities.Implementations;

public class TicketStatus : ITicketStatus
{
    private TicketStatus() { }

    public TicketStatus(int id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, "Ticket status name cannot be null or white space");

        Id = id;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
}