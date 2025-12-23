using TicketService.Entities.Interfaces;

namespace TicketService.Entities.Implementations;

public class Project : IProject
{
    private Project() { }

    public Project(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, "Project name cannot be null or white space");

        Name = name;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }

    public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>();
}