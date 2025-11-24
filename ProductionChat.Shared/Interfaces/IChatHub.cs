using MagicOnion;
using ProductionChat.Shared.DTOs;

namespace ProductionChat.Interfaces;

public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>
{
    Task JoinAsync(JoinRequest request);

    Task LeaveAsync();

    Task SendMessageAsync(string message);

    Task GenerateException(string message);
}