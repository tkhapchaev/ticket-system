using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TicketService.Configurations;
using TicketService.Dtos.Mappers;
using TicketService.Dtos.Request;
using TicketService.Dtos.Response;
using TicketService.Entities.Implementations;
using TicketService.Events;
using TicketService.Repositories;
using TicketService.Services.Interfaces;

namespace TicketService.Services.Implementations;

public class TicketService : ITicketService
{
    private readonly ApplicationContext _applicationContext;

    private readonly IEventProducer _eventBus;

    private readonly KafkaTopicOptions _kafkaTopicOptions;

    private readonly ILogger<TicketService> _logger;

    public TicketService(ApplicationContext applicationContext, IEventProducer eventBus, IOptions<KafkaTopicOptions> kafkaTopicOptions, ILogger<TicketService> logger)
    {
        _applicationContext = applicationContext;
        _eventBus = eventBus;
        _kafkaTopicOptions = kafkaTopicOptions.Value;
        _logger = logger;
    }

    public async Task<TicketDto> CreateAsync(CreateTicketDto createTicketDto, CancellationToken cancellationToken)
    {
        var ticket = new Ticket(
            createTicketDto.Title,
            createTicketDto.Description,
            createTicketDto.ReporterId,
            createTicketDto.PriorityId,
            createTicketDto.StatusId,
            createTicketDto.ProjectId);

        _applicationContext.Tickets.Add(ticket);
        await _applicationContext.SaveChangesAsync();

        await _eventBus.PublishAsync(
            _kafkaTopicOptions.TicketCreated,
            ticket.Id.ToString(),
            TicketEvents.TicketCreated(ticket),
            cancellationToken
        );

        _logger.LogInformation($"Ticket {ticket.Title} (id: {ticket.Id}) created");

        return TicketMapper.MapToDto(ticket);
    }

    public async Task<IEnumerable<TicketDto>> GetAllAsync()
        => await _applicationContext.Tickets
            .AsNoTracking()
            .Select(ticket => TicketMapper.MapToDto(ticket))
            .ToListAsync();

    public async Task<TicketDto?> GetByIdAsync(Guid id)
        => await _applicationContext.Tickets
            .AsNoTracking()
            .Where(ticket => ticket.Id == id)
            .Select(ticket => TicketMapper.MapToDto(ticket))
            .FirstOrDefaultAsync();

    public async Task<bool> UpdateAsync(Guid id, UpdateTicketDto updateTicketDto, CancellationToken cancellationToken)
    {
        var ticket = await _applicationContext.Tickets.FindAsync(id);

        if (ticket == null)
            return false;

        ticket.Update(updateTicketDto.Title, updateTicketDto.Description, updateTicketDto.PriorityId, updateTicketDto.StatusId);
        await _applicationContext.SaveChangesAsync();

        await _eventBus.PublishAsync(
            _kafkaTopicOptions.TicketUpdated,
            ticket.Id.ToString(),
            TicketEvents.TicketUpdated(ticket),
            cancellationToken
        );

        _logger.LogInformation($"Ticket {ticket.Title} (id: {ticket.Id}) updated");

        return true;
    }

    public async Task<bool> AssignAsync(Guid id, Guid? assigneeId, CancellationToken cancellationToken)
    {
        var ticket = await _applicationContext.Tickets.FindAsync(id);

        if (ticket == null)
            return false;

        ticket.Assign(assigneeId);
        await _applicationContext.SaveChangesAsync();

        await _eventBus.PublishAsync(
            _kafkaTopicOptions.TicketAssigned,
            ticket.Id.ToString(),
            TicketEvents.TicketAssigned(ticket),
            cancellationToken
        );

        var user = await _applicationContext.Users.FindAsync(assigneeId);

        _logger.LogInformation($"Ticket {ticket.Title} (id: {ticket.Id}) assigned to user {user.Login} (id: {user.Id})");

        return true;
    }
}