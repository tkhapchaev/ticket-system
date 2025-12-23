using Confluent.Kafka;
using NotificationService.Configurations;
using NotificationService.Services.Implementations;
using NotificationService.Services.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaTopicOptions>(builder.Configuration.GetSection("Kafka:Topics"));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var consumerConfig = new ConsumerConfig
    {
        BootstrapServers = configuration["Kafka:BootstrapServers"],
        GroupId = configuration["Kafka:Consumer:GroupId"],
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false
    };

    return new ConsumerBuilder<string, string>(consumerConfig).Build();
});

builder.Services.AddSingleton<INotificationSender, LogNotificationSender>();
builder.Services.AddHostedService<KafkaEventConsumer>();

await builder.Build().RunAsync();