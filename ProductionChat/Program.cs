using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductionChat.Server.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMagicOnion();
        services.AddSingleton<NotificationService>();
    });

var host = builder.Build();

// Запуск уведомлений
var notificationService = host.Services.GetRequiredService<NotificationService>();
notificationService.Start();

await host.RunAsync();
