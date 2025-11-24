using MagicOnion;
using ProductionChat.Interfaces;

public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>
{
    Task SendMessage(string message);
}