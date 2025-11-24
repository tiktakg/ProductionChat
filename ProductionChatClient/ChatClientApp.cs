using ProductionChat.Shared;
using Grpc.Net.Client;
using MagicOnion.Client;
using ProductionChat.Shared.Interfaces;

namespace ProductionChat.Client;

public class ChatClientApp : IChatHubReceiver
{
    private IChatHub? _hub;

    public async Task RunAsync()
    {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine()!.Trim();

        using var channel = GrpcChannel.ForAddress("http://localhost:5000");
        _hub = StreamingHubClient.Connect<IChatHub, IChatHubReceiver>(channel, this);

        await _hub.JoinAsync(name);

        Console.WriteLine("Подключено! Можно писать сообщения.");

        while (true)
        {
            var msg = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(msg)) continue;

            await _hub.SendMessageAsync(name, msg);
        }
    }

    public void OnReceiveMessage(string formattedMessage)
        => Console.WriteLine(formattedMessage);

    public void OnServerNotification(string message)
        => Console.WriteLine(message);
}
