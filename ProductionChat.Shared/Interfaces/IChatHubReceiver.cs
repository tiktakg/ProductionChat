namespace ProductionChat.Interfaces;

public interface IChatHubReceiver
{
    void OnReceiveMessage(string message);
}
