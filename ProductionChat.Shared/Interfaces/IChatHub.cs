using MagicOnion;
using ProductionChat.Interfaces;
using ProductionChat.Shared.DTOs;

public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>
{
    Task JoinAsync(JoinRequest request);

    Task LeaveAsync();

    Task SendMessageAsync(string message);

    Task GenerateException(string message);
}