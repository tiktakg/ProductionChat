using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using ProductionChat.Server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(endpointOptions =>
    {
        endpointOptions.Protocols = HttpProtocols.Http2;
    });
});
builder.Services.AddHostedService<NotificationService>();
builder.Services.AddMagicOnion()
    .UseRedisGroup(options =>
    {
        options.ConnectionString = "localhost:6379";
    });


var app = builder.Build();
app.MapMagicOnionService();

app.Run();