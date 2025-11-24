using MagicOnion.Server;
using MagicOnion.Server.Hubs;
using ProductionChat.Interfaces;

namespace ProductionChat.Server.Services;

public class NotificationService
{
    // Храним единственный экземпляр группы для глобального чата
    private IGroup<IChatHubReceiver>? _group;

    public void SetGroup(IGroup<IChatHubReceiver> group)
    {
        _group = group;
    }

    public void Start()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                if (_group != null)
                {
                    await _group.All.SendAsync(x =>
                        x.OnReceiveMessage("[SERVER]: периодическое уведомление"));
                }

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        });
    }
}
