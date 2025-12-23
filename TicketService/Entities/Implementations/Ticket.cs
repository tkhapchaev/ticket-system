using TicketService.Entities.Interfaces;

namespace TicketService.Entities.Implementations;

public class Ticket : ITicket
{
    private Ticket() { }

    public Ticket(string title, string description, Guid reporterId, int priorityId, int statusId, Guid projectId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title, "Ticket title cannot be null or white space");
        ArgumentException.ThrowIfNullOrWhiteSpace(description, "Ticket description cannot be null or white space");

        Title = title;
        Description = description;
        ReporterId = reporterId;
        PriorityId = priorityId;
        StatusId = statusId;
        ProjectId = projectId;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Title { get; private set; }
    public string Description { get; private set; }

    public Guid ReporterId { get; private set; }
    public User Reporter { get; private set; }

    public Guid? AssigneeId { get; private set; }
    public User? Assignee { get; private set; }

    public int PriorityId { get; private set; }
    public TicketPriority Priority { get; private set; }

    public int StatusId { get; private set; }
    public TicketStatus Status { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public void Update(string title, string description, int priorityId, int statusId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title, "Ticket title cannot be null or white space");
        ArgumentException.ThrowIfNullOrWhiteSpace(description, "Ticket description cannot be null or white space");

        Title = title;
        Description = description;
        PriorityId = priorityId;
        StatusId = statusId;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Assign(Guid? assigneeId)
    {
        AssigneeId = assigneeId;

        UpdatedAt = DateTime.UtcNow;
    }
}