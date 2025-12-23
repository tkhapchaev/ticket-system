namespace TicketService.Dtos.Request;

public record CreateTicketDto(
    string Title,
    string Description,
    Guid ReporterId,
    int PriorityId,
    int StatusId,
    Guid ProjectId
);