using System.Runtime.CompilerServices;
using Grpc.Net.Client;
using MagicOnion.Client;
using MessagePack;
using MessagePack.Resolvers;
using ProductionChat.Interfaces;
using ProductionChat.Shared.DTOs;

if (!RuntimeFeature.IsDynamicCodeSupported)
{
    StaticCompositeResolver.Instance.Register(
        BuiltinResolver.Instance,
        PrimitiveObjectResolver.Instance,
        MagicOnionGeneratedClientInitializer.Resolver,
        StandardResolver.Instance
    );
    MessagePackSerializer.DefaultOptions = MessagePackSerializer.DefaultOptions.WithResolver(StaticCompositeResolver.Instance);
}
//var channel = GrpcChannel.ForAddress("http://localhost:5000"); 
var channel = GrpcChannel.ForAddress("http://chat-server:80"); // Для запуска  через Docker
var sessionId = Guid.NewGuid();
Console.WriteLine("Connecting...");
var hub = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(channel, new ChatHubReceiver(sessionId));
Console.WriteLine($"Connected: {sessionId}");

Console.Write("UserName: ");
var userName = Console.ReadLine();

Console.WriteLine($"Join:UserName={userName}");
await hub.JoinAsync(new JoinRequest() { UserName = userName });
Console.WriteLine($"Joined");

while (true)
{
    var message = Console.ReadLine();
    await hub.SendMessageAsync(message);
}

[MagicOnionClientGeneration(typeof(IChatHub))]
partial class MagicOnionGeneratedClientInitializer;

class ChatHubReceiver(Guid sessionId) : IChatHubReceiver
{
    public void OnJoin(string name)
    {
        Console.WriteLine($"<Event> Join: {name}");
    }

    public void OnLeave(string name)
    {
        Console.WriteLine($"<Event> Leave: {name}");
    }

    public void OnSendMessage(MessageResponse message)
    {
        Console.WriteLine($"{message.UserName}: {message.Message}");
    }
}