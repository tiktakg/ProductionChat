using ProductionChat.Shared.DTOs;

namespace ProductionChat.Interfaces;

public interface IChatHubReceiver
{
    void OnJoin(string name);

    void OnLeave(string name);

    void OnSendMessage(MessageResponse message);

}