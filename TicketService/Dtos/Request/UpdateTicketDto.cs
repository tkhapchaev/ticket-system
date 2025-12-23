namespace TicketService.Dtos.Request;

public record UpdateTicketDto(
    string Title,
    string Description,
    int PriorityId,
    int StatusId
);