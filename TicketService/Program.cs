using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketService.Configurations;
using TicketService.Repositories;
using TicketService.Services.Implementations;
using TicketService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaTopicOptions>(builder.Configuration.GetSection("Kafka:Topics"));

builder.Services.AddScoped<ITicketService, TicketService.Services.Implementations.TicketService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITicketPriorityService, TicketPriorityService>();
builder.Services.AddScoped<ITicketStatusService, TicketStatusService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["Swagger:Title"] ?? "TicketService",
        Version = builder.Configuration["Swagger:Version"] ?? "v1"
    });
});

builder.Services.AddDbContext<ApplicationContext>(dbContextOptionsBuilder => dbContextOptionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddSingleton(_ =>
{
    var producerConfig = new ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
        Acks = Acks.All
    };

    return new ProducerBuilder<string, string>(producerConfig).Build();
});

builder.Services.AddScoped<IEventProducer, KafkaEventProducer>();
var webApplication = builder.Build();

using (var serviceScope = webApplication.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
}

webApplication.UseSwagger();
webApplication.UseSwaggerUI();
webApplication.MapControllers();
webApplication.Run();