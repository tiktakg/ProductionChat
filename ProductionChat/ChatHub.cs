using Cysharp.Runtime.Multicast;
using MagicOnion;
using MagicOnion.Server.Hubs;
using ProductionChat.Interfaces;
using ProductionChat.Shared.DTOs;

namespace ProductionChat.Server
{
    public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub
    {
        private IGroup<IChatHubReceiver> room;
        private string myName;
        private readonly IMulticastSyncGroup<Guid, IChatHubReceiver> roomForAll;

        public ChatHub(IMulticastGroupProvider groupProvider)
        {
            roomForAll = groupProvider.GetOrAddSynchronousGroup<Guid, IChatHubReceiver>("All");
        }

        public async Task JoinAsync(JoinRequest request)
        {
            this.room = await this.Group.AddAsync(request.RoomName);

            this.myName = request.UserName;

            this.room.All.OnJoin(request.UserName);
        }


        public async Task LeaveAsync()
        {
            if (this.room is not null)
            {
                await this.room.RemoveAsync(this.Context);
                this.room.All.OnLeave(this.myName);
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (this.room is not null)
            {
                if (message.StartsWith("/global ", StringComparison.InvariantCultureIgnoreCase))
                {
                    var response = new MessageResponse { UserName = this.myName, Message = message.Substring("/global ".Length) };
                    this.roomForAll.All.OnSendMessage(response);
                }
                else
                {
                    var response = new MessageResponse { UserName = this.myName, Message = message };
                    this.room.All.OnSendMessage(response);
                }
            }

            await Task.CompletedTask;
        }

        public Task GenerateException(string message)
        {
            throw new Exception(message);
        }

       
        public Task SampleMethod(List<int> sampleList, Dictionary<int, string> sampleDictionary)
        {
            throw new System.NotImplementedException();
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