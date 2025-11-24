using MessagePack;


namespace ProductionChat.Shared.Interfaces
{
    [MessagePackObject]
    public class ChatMessage
    {
        [Key(0)] public string UserName { get; set; }
        [Key(1)] public string Message { get; set; }
    }
}
