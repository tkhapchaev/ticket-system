namespace TicketService.Dtos.Response;

public record TicketDto(
    Guid Id,
    string Title,
    string Description,
    Guid ReporterId,
    Guid? AssigneeId,
    int PriorityId,
    int StatusId,
    Guid ProjectId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);