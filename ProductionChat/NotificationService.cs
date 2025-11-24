using Cysharp.Runtime.Multicast;
using Microsoft.Extensions.Hosting;
using ProductionChat.Interfaces;
using ProductionChat.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionChat.Server
{
    public class NotificationService : BackgroundService
    {
        private readonly IMulticastGroupProvider _groupProvider;
        private IMulticastSyncGroup<Guid, IChatHubReceiver> _globalGroup;

        public NotificationService(IMulticastGroupProvider groupProvider)
        {
            _groupProvider = groupProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _globalGroup = _groupProvider.GetOrAddSynchronousGroup<Guid, IChatHubReceiver>("All");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // каждую минуту
                var message = new MessageResponse { UserName = "[SERVER]", Message = "Server notification: ..." };
                _globalGroup.All.OnSendMessage(message);
            }
        }
    }
}
