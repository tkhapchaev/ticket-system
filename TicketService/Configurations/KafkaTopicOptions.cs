namespace TicketService.Configurations;

public class KafkaTopicOptions
{
    public string TicketCreated { get; init; }
    public string TicketUpdated { get; init; }
    public string TicketAssigned { get; init; }
}