using TicketService.Entities.Implementations;

namespace TicketService.Entities.Interfaces;

public interface ITicket
{
    Guid Id { get; }

    string Title { get; }
    string Description { get; }

    Guid ReporterId { get; }
    User Reporter { get; }

    Guid? AssigneeId { get; }
    User? Assignee { get; }

    int PriorityId { get; }
    TicketPriority Priority { get; }

    int StatusId { get; }
    TicketStatus Status { get; }

    Guid ProjectId { get; }
    Project Project { get; }

    DateTime CreatedAt { get; }
    DateTime UpdatedAt { get; }

    void Update(string title, string description, int priorityId, int statusId);
    void Assign(Guid? assigneeId);
}