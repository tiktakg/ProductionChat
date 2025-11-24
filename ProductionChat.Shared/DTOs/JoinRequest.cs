using MessagePack;

namespace ProductionChat.Shared.DTOs;

[MessagePackObject]
public struct JoinRequest
{
    [Key(1)]
    public string UserName { get; set; }
}