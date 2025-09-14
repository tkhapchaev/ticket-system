using NotificationService.Interfaces;
using NotificationService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<INotificationSender, LogNotificationSender>();
builder.Services.AddHostedService<NotificationWorker>();

var host = builder.Build();
host.Run();