namespace TicketService.Entities;

public enum TicketPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public enum TicketStatus
{
    Open = 0,
    InProgress = 1,
    Resolved = 2,
    Closed = 3
}

public class Ticket
{
    public Ticket(string title, string description, TicketPriority priority, string reporterEmail)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException(nameof(title));
        }
        
        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException(nameof(description));
        }
        
        if (string.IsNullOrEmpty(reporterEmail))
        {
            throw new ArgumentException(nameof(reporterEmail));
        }
        
        Title = title;
        Description = description;
        Priority = priority;
        ReporterEmail = reporterEmail;
    }
    
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TicketPriority Priority { get; private set; }
    public TicketStatus Status { get; private set; } = TicketStatus.Open;
    public string ReporterEmail { get; private set; }
    public string? AssigneeEmail { get; private set; }
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public void UpdateCore(string title, string description, TicketPriority priority)
    {
        Title = title;
        Description = description;
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(TicketStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Assign(string assigneeEmail)
    {
        if (string.IsNullOrEmpty(assigneeEmail))
        {
            throw new ArgumentException(nameof(assigneeEmail));
        }
        
        AssigneeEmail = assigneeEmail;
        UpdatedAt = DateTime.UtcNow;
    }
}
