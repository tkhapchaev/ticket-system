using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketService.Infrastructure;
using TicketService.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration["Swagger:Title"] ?? "Ticket Service",
        Version = builder.Configuration["Swagger:Version"] ?? "v1"
    });
});

builder.Services.AddDbContext<TicketsDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Pg")));
builder.Services.AddSingleton<IProducer<string, string>>(_ =>
{
    var cfg = new ProducerConfig
    {
        BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
    };
    return new ProducerBuilder<string, string>(cfg).Build();
});

builder.Services.AddScoped<IEventBus, KafkaEventBus>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketsDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();