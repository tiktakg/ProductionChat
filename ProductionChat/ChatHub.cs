using Cysharp.Runtime.Multicast;
using MagicOnion;
using MagicOnion.Server.Hubs;
using ProductionChat.Interfaces;
using ProductionChat.Shared.DTOs;

namespace ProductionChat.Server
{
    public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub
    {
        private string myName;
        private readonly IMulticastSyncGroup<Guid, IChatHubReceiver> roomForAll;

        public ChatHub(IMulticastGroupProvider groupProvider)
        {
            roomForAll = groupProvider.GetOrAddSynchronousGroup<Guid, IChatHubReceiver>("All");
        }

        public async Task JoinAsync(JoinRequest request)
        {
                
            this.myName = request.UserName;

            this.roomForAll.All.OnJoin(request.UserName);
        }


        public async Task LeaveAsync()
        {
            this.roomForAll.All.OnLeave(this.myName);
        }

        public async Task SendMessageAsync(string message)
        {
            var response = new MessageResponse { UserName = this.myName, Message = message };
            this.roomForAll.All.OnSendMessage(response);

            await Task.CompletedTask;
        }

        public Task GenerateException(string message)
        {
            throw new Exception(message);
        }

        protected override ValueTask OnConnecting()
        {
            Console.WriteLine($"client connected {this.Context.ContextId}");
            roomForAll.Add(ConnectionId, Client);
            return CompletedTask;
        }

        protected override ValueTask OnDisconnected()
        {
         
            roomForAll.Remove(ConnectionId);
            return CompletedTask;
        }
    }
}