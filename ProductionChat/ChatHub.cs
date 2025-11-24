using MagicOnion.Server.Hubs;
using ProductionChat.Interfaces;
using System.Text.RegularExpressions;

public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub
{
    private IGroup<IChatHubReceiver>? _group;
    private string _userName = "";

    protected override async ValueTask OnConnected()
    {
        _group = await Group.AddAsync("global-chat");
    }

    protected override async ValueTask OnDisconnected()
    {
        if (_group != null)
        {
            await _group.RemoveAsync(Context);
        }
    }

    public async Task SendMessage(string message)
    {
        if (_group != null)
        {
            Broadcast
            await _group.All.SendAsync(x => x.OnReceiveMessage($"{_userName}: {message}"));
        }
    }

    public Task SetUserName(string userName)
    {
        _userName = userName;
        return Task.CompletedTask;
    }
}
